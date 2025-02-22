namespace DataCat.Server.HttpModels.Responses.NotificationChannel;

public class NotificationChannelResponse
{
    public required Guid Id { get; init; }

    public required string Destination { get; init; }
    
    public required string Settings { get; init; }
}