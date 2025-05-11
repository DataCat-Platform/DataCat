namespace DataCat.Storage.Postgres.Snapshots;

public sealed record NotificationChannelGroupSnapshot
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public List<NotificationChannelSnapshot> Channels { get; init; } = [];
    public required string NamespaceId { get; init; } 
}

public static class NotificationChannelGroupSnapshotExtensions
{
    public static NotificationChannelGroupSnapshot Save(this NotificationChannelGroup notificationGroup)
    {
        return new NotificationChannelGroupSnapshot
        {
            Id = notificationGroup.Id.ToString(),
            Name = notificationGroup.Name,
            Channels = notificationGroup.NotificationChannels.Select(x => x.Save()).ToList(),
            NamespaceId = notificationGroup.NamespaceId.ToString()
        };
    }

    public static NotificationChannelGroup RestoreFromSnapshot(this NotificationChannelGroupSnapshot groupSnapshot,
        NotificationChannelManager notificationChannelManager)
    {
        var result = NotificationChannelGroup.Create(
            Guid.Parse(groupSnapshot.Id), 
            groupSnapshot.Name,
            groupSnapshot.Channels.Select(x => x.RestoreFromSnapshot(notificationChannelManager)).ToList(),
            Guid.Parse(groupSnapshot.NamespaceId));
        
        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(NotificationChannelGroupSnapshot));
    }
}
