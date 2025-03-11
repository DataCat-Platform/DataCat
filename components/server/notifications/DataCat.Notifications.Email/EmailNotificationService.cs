namespace DataCat.Notifications.Email;

public sealed class EmailNotificationService(EmailNotificationOption option) : INotificationService
{
    public Task SendNotificationAsync(AlertEntity alertEntity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}