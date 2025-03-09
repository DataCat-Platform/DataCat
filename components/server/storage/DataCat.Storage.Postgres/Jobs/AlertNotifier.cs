namespace DataCat.Storage.Postgres.Jobs;

#if DEBUG
[DisallowConcurrentExecution]
#endif
public sealed class AlertNotifier(
    IAlertMonitorService alertMonitorService,
    IDefaultRepository<AlertEntity, Guid> alertRepository,
    DataSourceManager dataSourceManager,
    INotificationService notificationService,
    UnitOfWork unitOfWork,
    ILogger<AlertNotifier> logger)
    : BaseBackgroundWorker(logger)
{
    protected override async Task RunAsync(CancellationToken stoppingToken = default)
    {
        TrackJobStart(nameof(AlertNotifier));
        await unitOfWork.StartTransactionAsync(stoppingToken);
        
        const int limit = 5;
        var triggeredAlerts = await alertMonitorService.GetTriggeredAlertsAsync(limit, stoppingToken);
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

        var task = Parallel.ForEachAsync(triggeredAlerts, parallelOptions, async (alert, token) =>
        {
            try
            {
                var metricClient = dataSourceManager.GetMetricClient(alert.QueryEntity.DataSourceEntity);
                var isTriggeredYet = await metricClient.CheckAlertTriggerAsync(alert.QueryEntity.RawQuery, token);
                
                if (isTriggeredYet)
                {
                    alert.SetFire();
                    await notificationService.SendNotificationAsync(alert, stoppingToken);
                }
                else
                {
                    alert.ResetAlert();
                }
                
                await alertChannel.Writer.WriteAsync(alert, token);
                logger.LogDebug("[{Job}] {Alert} was written to the channel", nameof(AlertNotifier), alert.Id);
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
                logger.LogDebug("[{Job}] {Alert} was read from the channel", nameof(AlertNotifier), alert.Id);
                await alertRepository.UpdateAsync(alert, stoppingToken);
            }

            await unitOfWork.CommitAsync(stoppingToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "[{Job}] Job failed", nameof(AlertNotifier));
            await unitOfWork.RollbackAsync(stoppingToken);
        }
        finally
        {
            TrackJobEnd(nameof(AlertNotifier));
        }
    }
}