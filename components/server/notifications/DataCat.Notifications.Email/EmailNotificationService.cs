namespace DataCat.Notifications.Email;

public sealed class EmailNotificationService(EmailNotificationOption option) : INotificationService
{
    public Task SendNotificationAsync(Alert alert, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}