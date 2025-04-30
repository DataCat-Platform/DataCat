namespace DataCat.Server.Application.Queries.NotificationChannels.Get;

public sealed record GetNotificationChannelResponse
{
    public required Guid Id { get; init; }
    public required string DestinationName { get; init; }
    public required string Settings { get; init; }
}

public static class GetNotificationChannelResponseExtensions
{
    public static GetNotificationChannelResponse ToResponse(this NotificationChannel alert)
    {
        return new GetNotificationChannelResponse
        {
            Id = alert.Id,
            DestinationName = alert.NotificationOption.NotificationDestination.Name,
            Settings = alert.NotificationOption.Settings
        };
    }
}