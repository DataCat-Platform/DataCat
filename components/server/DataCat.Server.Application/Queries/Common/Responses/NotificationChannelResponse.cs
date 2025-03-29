namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record NotificationChannelResponse
{
    public required Guid Id { get; init; }
    public required string DestinationName { get; init; }
    public required string Settings { get; init; }
}

public static class NotificationChannelResponseExtensions
{
    public static NotificationChannelResponse ToResponse(this NotificationChannelEntity alert)
    {
        return new NotificationChannelResponse
        {
            Id = alert.Id,
            DestinationName = alert.NotificationOption.NotificationDestination.Name,
            Settings = alert.NotificationOption.Settings
        };
    }
}