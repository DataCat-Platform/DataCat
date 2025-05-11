namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed class NotificationChannelGroupResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required List<NotificationChannelResponse> NotificationChannels { get; init; }
    public required Guid NamespaceId { get; init; }
}

public static class NotificationChannelGroupResponseExtensions
{
    public static NotificationChannelGroupResponse ToResponse(this NotificationChannelGroup notificationChannelGroup)
    {
        return new NotificationChannelGroupResponse
        {
            Id = notificationChannelGroup.Id.ToString(),
            Name = notificationChannelGroup.Name,
            NotificationChannels = notificationChannelGroup.NotificationChannels.Select(x => x.ToResponse()).ToList(),
            NamespaceId = notificationChannelGroup.NamespaceId
        };
    }
}