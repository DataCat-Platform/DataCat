namespace DataCat.Server.Application.Alerts;

public interface INotificationOptionFactory
{
    bool IsResponsibleFor(string notificationOptionName);
    
    Result<BaseNotificationOption> Create(NotificationDestination? destination, string settings);
    
    Task<Result<INotificationService>> CreateNotificationServiceAsync(
        BaseNotificationOption notificationOption,
        ISecretsProvider secretsProvider, 
        CancellationToken cancellationToken = default);
}