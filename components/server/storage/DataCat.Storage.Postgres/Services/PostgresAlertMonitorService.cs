namespace DataCat.Storage.Postgres.Services;

public class PostgresAlertMonitorService(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork,
    NotificationChannelManager notificationChannelManager)
    : IAlertMonitorService
{
    public async Task<IEnumerable<AlertEntity>> GetAlertsToCheckAsync(int limit = 5, CancellationToken token = default)
    {
        var parameters = new { Limit = limit };
        var sql = @$"
        SELECT 
            a.{Public.Alerts.AlertId},
            a.{Public.Alerts.AlertDescription},
            a.{Public.Alerts.AlertStatus},
            a.{Public.Alerts.AlertRawQuery},
            a.{Public.Alerts.AlertDataSourceId},
            a.{Public.Alerts.AlertNotificationChannelId},
            a.{Public.Alerts.AlertPreviousExecution},
            a.{Public.Alerts.AlertNextExecution},
            a.{Public.Alerts.AlertWaitTimeBeforeAlertingInTicks},
            a.{Public.Alerts.AlertRepeatIntervalInTicks},

            ds.{Public.DataSources.DataSourceId},
            ds.{Public.DataSources.DataSourceName},
            ds.{Public.DataSources.DataSourceType},
            ds.{Public.DataSources.DataSourceConnectionString},

            nc.{Public.NotificationChannels.NotificationChannelId},
            nc.{Public.NotificationChannels.NotificationSettings},
            nc.{Public.NotificationChannels.NotificationDestination}
        FROM
            {Public.AlertTable} a
        INNER JOIN
            {Public.DataSourceTable} ds ON a.{Public.Alerts.AlertDataSourceId} = {Public.DataSources.DataSourceId}
        INNER JOIN
            {Public.NotificationTable} nc ON {Public.NotificationChannels.NotificationChannelId} = {Public.Alerts.AlertNotificationChannelId}
        WHERE
            a.{Public.Alerts.AlertStatus} = {AlertStatus.InActive.Value}
            AND
            a.{Public.Alerts.AlertNextExecution} < NOW()
        ORDER BY
            a.{Public.Alerts.AlertNextExecution}
        LIMIT @Limit
        FOR UPDATE SKIP LOCKED;
        ";
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var alertsToCheck = await connection
            .QueryAsync<AlertSnapshot, DataSourceSnapshot, NotificationChannelSnapshot, AlertSnapshot>(
                sql,
                map: (alert, dataSource, notificationChannel) =>
                {
                    alert.AlertDataSource = dataSource;
                    alert.AlertNotificationChannel = notificationChannel;
                    return alert;
                },
                splitOn: $"{Public.DataSources.DataSourceId}, {Public.NotificationChannels.NotificationChannelId}",
                param: parameters,
                transaction: unitOfWork.Transaction);

        return alertsToCheck.Select(x => x.RestoreFromSnapshot(notificationChannelManager));
    }

    public async Task<IEnumerable<AlertEntity>> GetTriggeredAlertsAsync(int limit = 5, CancellationToken token = default)
    {
        var parameters = new { Limit = limit };
        
        var sql = @$"
        SELECT 
            a.{Public.Alerts.AlertId},
            a.{Public.Alerts.AlertDescription},
            a.{Public.Alerts.AlertStatus},
            a.{Public.Alerts.AlertRawQuery},
            a.{Public.Alerts.AlertDataSourceId},
            a.{Public.Alerts.AlertNotificationChannelId},
            a.{Public.Alerts.AlertPreviousExecution},
            a.{Public.Alerts.AlertNextExecution},
            a.{Public.Alerts.AlertWaitTimeBeforeAlertingInTicks},
            a.{Public.Alerts.AlertRepeatIntervalInTicks},

            ds.{Public.DataSources.DataSourceId},
            ds.{Public.DataSources.DataSourceName},
            ds.{Public.DataSources.DataSourceType},
            ds.{Public.DataSources.DataSourceConnectionString},

            nc.{Public.NotificationChannels.NotificationChannelId},
            nc.{Public.NotificationChannels.NotificationSettings},
            nc.{Public.NotificationChannels.NotificationDestination}
        FROM
            {Public.AlertTable} a
        INNER JOIN
            {Public.DataSourceTable} ds ON a.{Public.Alerts.AlertDataSourceId} = ds.{Public.DataSources.DataSourceId}
        INNER JOIN
            {Public.NotificationTable} nc ON nc.{Public.NotificationChannels.NotificationChannelId} = a.{Public.Alerts.AlertNotificationChannelId}
        WHERE
            a.{Public.Alerts.AlertStatus} IN ({AlertStatus.Warning.Value}, {AlertStatus.Fire.Value}, {AlertStatus.Muted.Value})
            AND
            a.{Public.Alerts.AlertNextExecution} < NOW()
        ORDER BY
            a.{Public.Alerts.AlertNextExecution}
        LIMIT @Limit
        FOR UPDATE SKIP LOCKED;
        ";

        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var firedAlerts = await connection
            .QueryAsync<AlertSnapshot, DataSourceSnapshot, NotificationChannelSnapshot, AlertSnapshot>(
                sql,
                map: (alert, dataSource, notificationChannel) =>
                {
                    alert.AlertDataSource = dataSource;
                    alert.AlertNotificationChannel = notificationChannel;
                    return alert;
                },
                splitOn: $"{Public.DataSources.DataSourceId}, {Public.NotificationChannels.NotificationChannelId}",
                param: parameters,
                transaction: unitOfWork.Transaction);

        return firedAlerts.Select(x => x.RestoreFromSnapshot(notificationChannelManager));
    }
}