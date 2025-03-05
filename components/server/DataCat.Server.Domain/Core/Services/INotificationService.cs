namespace DataCat.Server.Domain.Core.Services;

public interface INotificationService
{
    Task SendNotificationAsync(AlertEntity alertEntity, CancellationToken token = default);
}

public class NotificationService : INotificationService
{
    public Task SendNotificationAsync(AlertEntity alertEntity, CancellationToken token = default)
    {
        Console.WriteLine("SendNotificationAsync");
        return Task.CompletedTask; 
    }
}