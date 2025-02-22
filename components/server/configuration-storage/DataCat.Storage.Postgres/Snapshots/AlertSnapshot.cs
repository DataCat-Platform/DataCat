namespace DataCat.Storage.Postgres.Snapshots;

public sealed class AlertSnapshot
{
    public const string AlertTable = "alerts";
    public const string Alert_DataSourceId = "alert_data_source_id";
    public const string Alert_NotificationChannelId = "alert_notification_channel_id";
    
    public required string AlertId { get; init; }
    public required string AlertDescription { get; init; }
    public required string AlertRawQuery { get; init; }
    public required DataSourceSnapshot AlertDataSource { get; set; }
    public string AlertDataSourceId => AlertDataSource.DataSourceId;
    public required NotificationChannelSnapshot AlertNotificationChannel { get; set; }
    public string AlertNotificationChannelId => AlertNotificationChannel.NotificationChannelId;
}

public static class AlertSnapshotExtensions
{
    public static AlertSnapshot ReadAlert(this DbDataReader reader)
    {
        return new AlertSnapshot
        {
            AlertId = reader.GetString(reader.GetOrdinal(Public.Alerts.AlertId)),
            AlertDescription = reader.GetString(reader.GetOrdinal(Public.Alerts.AlertDescription)),
            AlertRawQuery = reader.GetString(reader.GetOrdinal(Public.Alerts.AlertRawQuery)),
            AlertDataSource = new DataSourceSnapshot
            {
                DataSourceId = reader.GetString(reader.GetOrdinal(Public.DataSources.DataSourceId)),
                DataSourceName = reader.GetString(reader.GetOrdinal(Public.DataSources.DataSourceName)),
                DataSourceType = reader.GetInt32(reader.GetOrdinal(Public.DataSources.DataSourceType)),
                DataSourceConnectionString = reader.GetString(reader.GetOrdinal(Public.DataSources.DataSourceConnectionString)),
            },
            AlertNotificationChannel = new NotificationChannelSnapshot
            {
                NotificationChannelId = reader.GetString(reader.GetOrdinal(Public.NotificationChannels.NotificationChannelId)),
                NotificationDestination = reader.GetInt32(reader.GetOrdinal(Public.NotificationChannels.NotificationDestination)),
                NotificationSettings = reader.GetString(reader.GetOrdinal(Public.NotificationChannels.NotificationSettings)),
            }
        };
    }
    
    public static AlertSnapshot Save(this AlertEntity alert)
    {
        return new AlertSnapshot
        {
            AlertId = alert.Id.ToString(),
            AlertDescription = alert.Description,
            AlertRawQuery = alert.QueryEntity.RawQuery,
            AlertDataSource = alert.QueryEntity.DataSourceEntity.Save(),
            AlertNotificationChannel = alert.NotificationChannelEntity.Save()
        };
    }

    public static AlertEntity RestoreFromSnapshot(this AlertSnapshot snapshot)
    {
        var result = AlertEntity.Create(
            Guid.Parse(snapshot.AlertId),
            snapshot.AlertDescription,
            QueryEntity.Create(snapshot.AlertDataSource.RestoreFromSnapshot(), snapshot.AlertRawQuery).Value,
            snapshot.AlertNotificationChannel.RestoreFromSnapshot()
        );

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(AlertEntity));
    }
}