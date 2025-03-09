namespace DataCat.Server.Api.Mappings;

public static class NotificationChannelMappings
{
    public static AddNotificationCommand ToAddCommand(this AddNotificationChannelRequest request)
    {
        return new AddNotificationCommand
        {
            DestinationType = request.DestinationType,
            Settings = request.Settings
        };
    }

    public static UpdateNotificationCommand ToUpdateCommand(this UpdateNotificationChannelRequest request, string notificationChannelId)
    {
        return new UpdateNotificationCommand
        {
            NotificationChannelId = notificationChannelId,
            DestinationType = request.DestinationType,
            Settings = request.Settings
        };
    }

    public static NotificationChannelResponse ToResponse(this NotificationChannelEntity alert)
    {
        return new NotificationChannelResponse
        {
            Id = alert.Id,
            Destination = alert.NotificationOption.NotificationDestination.Name,
            Settings = alert.NotificationOption.Settings
        };
    }
}