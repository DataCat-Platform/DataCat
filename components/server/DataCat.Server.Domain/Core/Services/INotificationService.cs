namespace DataCat.Server.Domain.Core.Services;

public interface INotificationService
{
    Task SendNotificationAsync(AlertEntity alertEntity, CancellationToken token = default);
}

public class NotificationService : INotificationService
{
    public Task SendNotificationAsync(AlertEntity alertEntity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}