namespace DataCat.Server.HttpModels.Requests.NotificationChannel;

public class AddNotificationChannelRequest
{
    public required int DestinationType { get; init; }
    
    public required string Settings { get; init; }
}