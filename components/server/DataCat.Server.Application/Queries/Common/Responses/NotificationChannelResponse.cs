namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record NotificationChannelResponse
{
    public required int Id { get; init; }
    public required string DestinationName { get; init; }
    public required string Settings { get; init; }
}

public static class NotificationChannelResponseExtensions
{
    public static NotificationChannelResponse ToResponse(this NotificationChannel notificationChannel)
    {
        return new NotificationChannelResponse
        {
            Id = notificationChannel.Id,
            DestinationName = notificationChannel.NotificationOption.NotificationDestination.Name,
            Settings = notificationChannel.NotificationOption.Settings
        };
    }
}