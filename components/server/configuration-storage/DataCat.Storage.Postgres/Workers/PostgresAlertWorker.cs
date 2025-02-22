namespace DataCat.Storage.Postgres.Workers;

public class PostgresAlertWorker(
    INotificationService notificationService,
    IAlertMonitor alertMonitor,
    ILogger<PostgresAlertWorker> logger) 
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.CompletedTask;
    }
}