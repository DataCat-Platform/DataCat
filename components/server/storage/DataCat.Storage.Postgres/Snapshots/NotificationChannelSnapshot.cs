namespace DataCat.Storage.Postgres.Snapshots;

public sealed record NotificationChannelSnapshot
{
    public required string Id { get; init; }
    public required NotificationDestinationSnapshot Destination { get; set; }
    public int? DestinationId => Destination.Id;
    public required string Settings { get; init; }
}

public static class NotificationChannelSnapshotExtensions
{
    public static NotificationChannelSnapshot Save(this NotificationChannel notification)
    {
        return new NotificationChannelSnapshot
        {
            Id = notification.Id.ToString(),
            Destination = notification.NotificationOption.NotificationDestination.Save(),
            Settings = notification.NotificationOption.Settings
        };
    }

    public static NotificationChannel RestoreFromSnapshot(this NotificationChannelSnapshot snapshot, NotificationChannelManager notificationChannelManager)
    {
        var destination = snapshot.Destination.RestoreFromSnapshot();
        
        var result = NotificationChannel.Create(
            Guid.Parse(snapshot.Id),
            notificationChannelManager.GetNotificationChannelFactory(destination).Create(destination, snapshot.Settings).Value
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(NotificationChannel));
    }
}