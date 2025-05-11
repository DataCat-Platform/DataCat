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
    ILogger<AlertNotifier> logger,
    IMetricsContainer metricsContainer)
    : BaseBackgroundWorker(logger, metricsContainer)
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
                using var metricClient = dataSourceManager.GetMetricsClient(alert.ConditionQuery.DataSource.Name);
                if (metricClient is null)
                {
                    logger.LogError(
                        "[{Job}] Failed to create metrics client for DataSource '{DataSourceName}'",
                        nameof(AlertNotifier),
                        alert.ConditionQuery.DataSource.Name
                    );
                    return;
                }
                
                var isTriggered = await metricClient.CheckAlertTriggerAsync(alert.ConditionQuery.RawQuery, token);
                
                if (isTriggered)
                {
                    var hasErrors = false;
                    var allErrors = new List<ErrorInfo>();

                    foreach (var channel in alert.NotificationChannelGroup.NotificationChannels)
                    {
                        var notificationOptionFactory = notificationChannelManager
                            .GetNotificationChannelFactory(channel.NotificationOption.NotificationDestination);

                        var optionResult = notificationOptionFactory.Create(
                            channel.NotificationOption.NotificationDestination,
                            channel.NotificationOption.Settings);

                        if (optionResult.IsFailure)
                        {
                            hasErrors = true;
                            allErrors.AddRange(optionResult.Errors!);
                            continue;
                        }

                        var notificationServiceResult = await notificationOptionFactory
                            .CreateNotificationServiceAsync(optionResult.Value, serviceProvider, cancellationToken: token);

                        if (notificationServiceResult.IsFailure)
                        {
                            hasErrors = true;
                            allErrors.AddRange(notificationServiceResult.Errors!);
                            continue;
                        }

                        var stopwatch = Stopwatch.StartNew();
                        try
                        {
                            await notificationServiceResult.Value.SendNotificationAsync(alert, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError("[{Job}] {Alert} had errors during sending alert to notification channel {@NotificationChannel}. Exception: {@Exception}",
                                nameof(AlertNotifier), alert.Id, optionResult.Value.NotificationDestination.Name, ex.Message);
                            hasErrors = true;                        
                        }
                        stopwatch.Stop();

                        var destinationName = channel.NotificationOption.NotificationDestination.Name;

                        metricsContainer.AddNotificationSent(destinationName, isSuccess: true);
                        metricsContainer.AddNotificationDeliveryTime(stopwatch.ElapsedMilliseconds, destinationName);
                    }

                    if (hasErrors)
                    {
                        alert.SetErrorStatus();
                        logger.LogError("[{Job}] {Alert} had errors during alerting. Errors: {@Errors}",
                            nameof(AlertNotifier), alert.Id, allErrors);
                    }
                    else
                    {
                        alert.CommitAlertExecution();
                    }
                    
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
                foreach (var channel in alert.NotificationChannelGroup.NotificationChannels)
                {
                    metricsContainer.AddNotificationSent(channel.NotificationOption.NotificationDestination.Name, isSuccess: false);
                }
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