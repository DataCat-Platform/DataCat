namespace DataCat.Storage.Postgres.Snapshots;

public sealed class AlertSnapshot
{
    public required string Id { get; init; }
    public required string? Description { get; init; }
    public required int Status { get; init; }
    public required string RawQuery { get; init; }
    public required DataSourceSnapshot DataSource { get; set; }
    public string DataSourceId => DataSource.Id;
    public required NotificationChannelSnapshot NotificationChannel { get; set; }
    public string NotificationChannelId => NotificationChannel.Id;
    public required DateTime PreviousExecution { get; init; }
    public required DateTime NextExecution { get; init; }
    public required long WaitTimeBeforeAlertingInTicks { get; init; }
    public required long RepeatIntervalInTicks { get; init; }
}

public static class AlertSnapshotExtensions
{
    public static AlertSnapshot Save(this AlertEntity alert)
    {
        return new AlertSnapshot
        {
            Id = alert.Id.ToString(),
            Description = alert.Description,
            Status = alert.Status.Value,
            RawQuery = alert.QueryEntity.RawQuery,
            DataSource = alert.QueryEntity.DataSourceEntity.Save(),
            NotificationChannel = alert.NotificationChannelEntity.Save(),
            PreviousExecution = alert.PreviousExecution.DateTime,
            NextExecution = alert.NextExecution.DateTime,
            RepeatIntervalInTicks = alert.RepeatInterval.Ticks,
            WaitTimeBeforeAlertingInTicks = alert.WaitTimeBeforeAlerting.Ticks
        };
    }

    public static AlertEntity RestoreFromSnapshot(this AlertSnapshot snapshot, NotificationChannelManager notificationChannelManager)
    {
        var result = AlertEntity.Create(
            Guid.Parse(snapshot.Id),
            snapshot.Description,
            QueryEntity.Create(snapshot.DataSource.RestoreFromSnapshot(), snapshot.RawQuery).Value,
            AlertStatus.FromValue(snapshot.Status),
            snapshot.NotificationChannel.RestoreFromSnapshot(notificationChannelManager),
            snapshot.PreviousExecution,
            snapshot.NextExecution,
            TimeSpan.FromTicks(snapshot.WaitTimeBeforeAlertingInTicks),
            TimeSpan.FromTicks(snapshot.RepeatIntervalInTicks)
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(AlertEntity));
    }
}