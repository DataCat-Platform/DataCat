namespace DataCat.Server.Domain.Core.ValueObjects;

public abstract record BaseNotificationOption
{
    public NotificationDestination NotificationDestination = null!;
    public abstract string Settings { get; }
}
