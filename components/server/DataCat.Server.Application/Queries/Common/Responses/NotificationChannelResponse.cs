namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record NotificationChannelResponse
{
    public required int Id { get; init; }
    public required string DestinationName { get; init; }
    public required string Settings { get; init; }
}

public static class NotificationChannelResponseExtensions
{
    public static NotificationChannelResponse ToResponse(this NotificationChannel notificationChannelGroup)
    {
        return new NotificationChannelResponse
        {
            Id = notificationChannelGroup.Id,
            DestinationName = notificationChannelGroup.NotificationOption.NotificationDestination.Name,
            Settings = notificationChannelGroup.NotificationOption.Settings
        };
    }
}