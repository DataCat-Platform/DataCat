namespace DataCat.Server.Application.Alerts;

public interface INotificationOptionFactory
{
    NotificationDestination NotificationDestination { get; }
    
    Result<BaseNotificationOption> Create(string settings);
}