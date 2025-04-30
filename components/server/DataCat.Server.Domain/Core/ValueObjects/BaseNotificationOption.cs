namespace DataCat.Server.Domain.Core.ValueObjects;

public abstract record BaseNotificationOption
{
    public NotificationDestination NotificationDestination = null!;
    public readonly string Settings = null!;
}
