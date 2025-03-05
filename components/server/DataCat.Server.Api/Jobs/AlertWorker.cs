namespace DataCat.Server.Api.Jobs;

public class AlertWorker(
    INotificationService notificationService,
    IAlertMonitor alertMonitor,
    ILogger<AlertWorker> logger) 
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("AlertWorker running.");
        await Task.CompletedTask;
    }
}