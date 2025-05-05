namespace DataCat.Storage.Postgres.Snapshots;

public sealed record AlertSnapshot
{
    public required string Id { get; init; }
    public required string? Description { get; init; }
    public required int Status { get; init; }
    public required string ConditionQuery { get; init; }
    public required DataSourceSnapshot DataSource { get; set; }
    public string DataSourceId => DataSource.Id;
    public required NotificationChannelGroupSnapshot NotificationChannelGroup { get; set; }
    public string NotificationChannelGroupId => NotificationChannelGroup.Id;
    public required DateTime PreviousExecution { get; init; }
    public required DateTime NextExecution { get; init; }
    public required long WaitTimeBeforeAlertingInTicks { get; init; }
    public required long RepeatIntervalInTicks { get; init; }
    public required List<Tag> Tags { get; init; }
}

public static class AlertSnapshotExtensions
{
    public static AlertSnapshot Save(this Alert alert)
    {
        return new AlertSnapshot
        {
            Id = alert.Id.ToString(),
            Description = alert.Template,
            Status = alert.Status.Value,
            ConditionQuery = alert.ConditionQuery.RawQuery,
            DataSource = alert.ConditionQuery.DataSource.Save(),
            NotificationChannelGroup = alert.NotificationChannelGroup.Save(),
            PreviousExecution = alert.PreviousExecution.DateTime,
            NextExecution = alert.NextExecution.DateTime,
            RepeatIntervalInTicks = alert.Schedule.RepeatInterval.Ticks,
            WaitTimeBeforeAlertingInTicks = alert.Schedule.WaitTimeBeforeAlerting.Ticks,
            Tags = alert.Tags.ToList(),
        };
    }

    public static Alert RestoreFromSnapshot(this AlertSnapshot snapshot, NotificationChannelManager notificationChannelManager)
    {
        var result = Alert.Create(
            id: Guid.Parse(snapshot.Id),
            snapshot.Description,
            Query.Create(snapshot.DataSource.RestoreFromSnapshot(), snapshot.ConditionQuery).Value,
            AlertStatus.FromValue(snapshot.Status),
            snapshot.NotificationChannelGroup.RestoreFromSnapshot(notificationChannelManager),
            previousExecution:snapshot.PreviousExecution,
            nextExecution: snapshot.NextExecution,
            TimeSpan.FromTicks(snapshot.WaitTimeBeforeAlertingInTicks),
            TimeSpan.FromTicks(snapshot.RepeatIntervalInTicks),
            snapshot.Tags
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(Alert));
    }
}