using DataCat.Server.Application.Telemetry.Metrics;

namespace DataCat.Storage.Postgres.Jobs;

#if DEBUG
[DisallowConcurrentExecution]
#endif
public sealed class AlertNotifier(
    IAlertMonitorService alertMonitorService,
    IAlertRepository alertRepository,
    DataSourceManager dataSourceManager,
    NotificationChannelManager notificationChannelManager,
    ISecretsProvider serviceProvider,
    UnitOfWork unitOfWork,
    ILogger<AlertNotifier> logger)
    : BaseBackgroundWorker(logger)
{
    protected override string JobName => nameof(AlertNotifier);

    protected override async Task RunAsync(CancellationToken stoppingToken = default)
    {
        await unitOfWork.StartTransactionAsync(stoppingToken);
        
        const int limit = 5;
        var triggeredAlerts = await alertMonitorService.GetTriggeredAlertsAsync(limit, stoppingToken);
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

        var task = Parallel.ForEachAsync(triggeredAlerts, parallelOptions, async (alert, token) =>
        {
            try
            {
                var metricClient = dataSourceManager.GetMetricsClient(alert.Query.DataSource.Name);
                if (metricClient is null)
                {
                    logger.LogError(
                        "[{Job}] Failed to create metrics client for DataSource '{DataSourceName}'",
                        nameof(AlertNotifier),
                        alert.Query.DataSource.Name
                    );
                    return;
                }
                
                var isTriggeredYet = await metricClient.CheckAlertTriggerAsync(alert.Query.RawQuery, token);
                
                if (isTriggeredYet)
                {
                    alert.SetFire();
                    var notificationOptionFactory = notificationChannelManager.GetNotificationChannelFactory(alert.NotificationChannel.NotificationOption.NotificationDestination);
                    
                    var optionResult =
                        notificationOptionFactory.Create(
                            alert.NotificationChannel.NotificationOption.NotificationDestination, 
                            alert.NotificationChannel.NotificationOption.Settings);

                    if (optionResult.IsFailure)
                    {
                        logger.LogError("[{Job}] {Alert} has errors during alerting. Error: {@Errors}", nameof(AlertNotifier), alert.Id, optionResult.Errors);
                        await alertChannel.Writer.WriteAsync(alert, token);
                        return;
                    }

                    var notificationServiceResult =
                        await notificationOptionFactory.CreateNotificationServiceAsync(optionResult.Value, serviceProvider, cancellationToken: token);

                    if (notificationServiceResult.IsFailure)
                    {
                        logger.LogError("[{Job}] {Alert} has errors during alerting. Errors: {@Errors}", nameof(AlertNotifier), alert.Id, optionResult.Errors);
                        await alertChannel.Writer.WriteAsync(alert, token);
                        return;
                    }
                        
                    await notificationServiceResult.Value.SendNotificationAsync(alert, stoppingToken);
                    logger.LogWarning("[{Job}] Alert: {Alert} is fired", nameof(AlertNotifier), alert.Id);
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
    }
}