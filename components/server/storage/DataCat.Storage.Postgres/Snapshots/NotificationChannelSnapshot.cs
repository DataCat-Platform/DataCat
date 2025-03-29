namespace DataCat.Storage.Postgres.Snapshots;

public sealed class NotificationChannelSnapshot
{
    public required string Id { get; init; }
    public required int DestinationId { get; init; }
    public required string Settings { get; init; }
}

public static class NotificationChannelSnapshotExtensions
{
    public static NotificationChannelSnapshot Save(this NotificationChannelEntity notification)
    {
        return new NotificationChannelSnapshot
        {
            Id = notification.Id.ToString(),
            DestinationId = notification.NotificationOption.NotificationDestination.Value,
            Settings = notification.NotificationOption.Settings
        };
    }

    public static NotificationChannelEntity RestoreFromSnapshot(this NotificationChannelSnapshot snapshot, NotificationChannelManager notificationChannelManager)
    {
        var destination = NotificationDestination.FromValue(snapshot.DestinationId);
        
        var result = NotificationChannelEntity.Create(
            Guid.Parse(snapshot.Id),
            destination,
            notificationChannelManager.GetNotificationChannelFactory(destination).Create(snapshot.Settings).Value
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(NotificationChannelEntity));
    }
}