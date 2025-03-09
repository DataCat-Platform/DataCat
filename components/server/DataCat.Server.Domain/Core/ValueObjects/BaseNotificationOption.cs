namespace DataCat.Server.Domain.Core.ValueObjects;

public abstract record BaseNotificationOption
{
    public abstract NotificationDestination NotificationDestination { get; }
    public abstract string Settings { get; }
}
