namespace DataCat.Storage.Postgres.Jobs;

#if DEBUG
[DisallowConcurrentExecution]
#endif
public sealed class AlertChecker(
    IAlertMonitorService alertMonitorService,
    IAlertRepository alertRepository,
    DataSourceManager dataSourceManager,
    UnitOfWork unitOfWork,
    ILogger<AlertChecker> logger) 
    : BaseBackgroundWorker(logger)
{
    protected override async Task RunAsync(CancellationToken stoppingToken = default)
    {
        TrackJobStart(nameof(AlertChecker));
        await unitOfWork.StartTransactionAsync(stoppingToken);
        
        // TODO: Change hardcoded top argument
        const int limit = 5;
        var alerts = await alertMonitorService.GetAlertsToCheckAsync(limit, stoppingToken);
        var alertChannel = Channel.CreateBounded<AlertEntity>(new BoundedChannelOptions(limit)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });
        
        var parallelOptions =
#if DEBUG
            new ParallelOptions { MaxDegreeOfParallelism = 1, CancellationToken = stoppingToken };
#else
            new ParallelOptions { CancellationToken = stoppingToken };
#endif

        var task = Parallel.ForEachAsync(alerts, parallelOptions, async (alert, token) =>
        {
            try
            {
                var metricClient = dataSourceManager.GetMetricClient(alert.QueryEntity.DataSourceEntity);
                var isTriggered = await metricClient.CheckAlertTriggerAsync(alert.QueryEntity.RawQuery, token);
                if (isTriggered)
                {
                    alert.SetWarningStatus();
                    logger.LogWarning("[{Job}] Alert: {Alert} switched to the warning status", nameof(AlertChecker), alert.Id);
                }
                else
                {
                    alert.CommitAlertExecution();
                }
                
                logger.LogInformation("[{Job}] Alert: {Alert} was written to the channel", nameof(AlertChecker), alert.Id);
                await alertChannel.Writer.WriteAsync(alert, token);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "{Job} failed", nameof(AlertChecker));
            }
        });

        _ = task.ContinueWith(_ =>
        {
            alertChannel.Writer.Complete();
        }, stoppingToken);

        try
        {
            await foreach (var alert in alertChannel.Reader.ReadAllAsync(stoppingToken))
            {
                logger.LogInformation("[{Job}] Alert: {Alert} was read from the channel", nameof(AlertNotifier), alert.Id);
                await alertRepository.UpdateAsync(alert, stoppingToken);
            }

            await unitOfWork.CommitAsync(stoppingToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "[{Job}] Job failed", nameof(AlertChecker));
            await unitOfWork.RollbackAsync(stoppingToken);
        }
        finally
        {
            TrackJobEnd(nameof(AlertChecker));
        }
    }
}