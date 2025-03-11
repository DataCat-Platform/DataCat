namespace DataCat.Storage.Postgres.Snapshots;

public sealed class NotificationChannelSnapshot
{
    public const string NotificationChannelTable = "notification_channel";
    
    public required string NotificationChannelId { get; init; }
    public required int NotificationDestination { get; init; }
    public required string NotificationSettings { get; init; }
}

public static class NotificationChannelSnapshotExtensions
{
    public static NotificationChannelSnapshot ReadNotificationChannel(this DbDataReader reader)
    {
        return new NotificationChannelSnapshot
        {
            NotificationChannelId = reader.GetString(reader.GetOrdinal(Public.NotificationChannels.NotificationChannelId)),
            NotificationDestination = reader.GetInt32(reader.GetOrdinal(Public.NotificationChannels.NotificationDestination)),
            NotificationSettings = reader.GetString(reader.GetOrdinal(Public.NotificationChannels.NotificationSettings)),
        };
    }
    
    public static NotificationChannelSnapshot Save(this NotificationChannelEntity notification)
    {
        return new NotificationChannelSnapshot
        {
            NotificationChannelId = notification.Id.ToString(),
            NotificationDestination = notification.NotificationOption.NotificationDestination.Value,
            NotificationSettings = notification.NotificationOption.Settings
        };
    }

    public static NotificationChannelEntity RestoreFromSnapshot(this NotificationChannelSnapshot snapshot, NotificationChannelManager notificationChannelManager)
    {
        var destination = NotificationDestination.FromValue(snapshot.NotificationDestination);
        
        var result = NotificationChannelEntity.Create(
            Guid.Parse(snapshot.NotificationChannelId),
            destination,
            notificationChannelManager.GetNotificationChannelFactory(destination).Create(snapshot.NotificationSettings).Value
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(NotificationChannelEntity));
    }
}