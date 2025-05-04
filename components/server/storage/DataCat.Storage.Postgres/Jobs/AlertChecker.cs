namespace DataCat.Storage.Postgres.Jobs;

#if DEBUG
[DisallowConcurrentExecution]
#endif
public sealed class AlertChecker(
    IAlertMonitorService alertMonitorService,
    IAlertRepository alertRepository,
    DataSourceManager dataSourceManager,
    UnitOfWork unitOfWork,
    ILogger<AlertChecker> logger,
    IMetricsContainer metricsContainer) 
    : BaseBackgroundWorker(logger, metricsContainer)
{
    protected override string JobName => nameof(AlertChecker);

    protected override async Task RunAsync(CancellationToken stoppingToken = default)
    {
        await unitOfWork.StartTransactionAsync(stoppingToken);
        
        // TODO: Change hardcoded top argument
        const int limit = 5;
        var alerts = await alertMonitorService.GetAlertsToCheckAsync(limit, stoppingToken);
        var alertChannel = Channel.CreateBounded<Alert>(new BoundedChannelOptions(limit)
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
                using var metricClient = dataSourceManager.GetMetricsClient(alert.Query.DataSource.Name);
                if (metricClient is null)
                {
                    logger.LogError(
                        "[{Job}] Failed to create metrics client for DataSource '{DataSourceName}'",
                        nameof(AlertChecker),
                        alert.Query.DataSource.Name
                    );
                    return;
                }
                
                var isTriggered = await metricClient.CheckAlertTriggerAsync(alert.Query.RawQuery, token);
                if (isTriggered)
                {
                    metricsContainer.AddAlertTriggered();
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
    }
}