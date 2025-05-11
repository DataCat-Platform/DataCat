namespace DataCat.Storage.Postgres.Snapshots;

public sealed record NotificationChannelSnapshot
{
    public required int Id { get; init; }
    public required string NotificationChannelGroupId { get; init; }
    public required NotificationDestinationSnapshot Destination { get; set; }
    public int? DestinationId => Destination.Id;
    public required string Settings { get; init; }
    public required string NamespaceId { get; init; }
}

public static class NotificationChannelSnapshotExtensions
{
    public static NotificationChannelSnapshot Save(this NotificationChannel notification)
    {
        return new NotificationChannelSnapshot
        {
            Id = notification.Id,
            NotificationChannelGroupId = notification.NotificationChannelGroupId.ToString(),
            Destination = notification.NotificationOption.NotificationDestination.Save(),
            Settings = notification.NotificationOption.Settings,
            NamespaceId = notification.NamespaceId.ToString(),
        };
    }

    public static NotificationChannel RestoreFromSnapshot(this NotificationChannelSnapshot snapshot, NotificationChannelManager notificationChannelManager)
    {
        var destination = snapshot.Destination.RestoreFromSnapshot();
        
        var result = NotificationChannel.Create(
            Guid.Parse(snapshot.NotificationChannelGroupId),
            notificationChannelManager.GetNotificationChannelFactory(destination).Create(destination, snapshot.Settings).Value,
            Guid.Parse(snapshot.NamespaceId),
            snapshot.Id
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(NotificationChannel));
    }
}