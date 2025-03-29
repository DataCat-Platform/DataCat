namespace DataCat.Storage.Postgres.Services;

public class PostgresAlertMonitorService(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork,
    NotificationChannelManager notificationChannelManager)
    : IAlertMonitorService
{
    public async Task<IEnumerable<AlertEntity>> GetAlertsToCheckAsync(int limit = 5, CancellationToken token = default)
    {
        var parameters = new { p_limit = limit };
        var sql = $"""
           SELECT 
               a.{Public.Alerts.Id}                                          {nameof(AlertSnapshot.Id)},
               a.{Public.Alerts.Description}                                 {nameof(AlertSnapshot.Description)},
               a.{Public.Alerts.Status}                                      {nameof(AlertSnapshot.Status)},
               a.{Public.Alerts.RawQuery}                                    {nameof(AlertSnapshot.RawQuery)},
               a.{Public.Alerts.DataSourceId}                                {nameof(AlertSnapshot.DataSourceId)},
               a.{Public.Alerts.NotificationChannelId}                       {nameof(AlertSnapshot.NotificationChannelId)},
               a.{Public.Alerts.PreviousExecution}                           {nameof(AlertSnapshot.PreviousExecution)},
               a.{Public.Alerts.NextExecution}                               {nameof(AlertSnapshot.NextExecution)},
               a.{Public.Alerts.WaitTimeBeforeAlertingInTicks}               {nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
               a.{Public.Alerts.RepeatIntervalInTicks}                       {nameof(AlertSnapshot.RepeatIntervalInTicks)},
   
               ds.{Public.DataSources.Id}                                    {nameof(DataSourceSnapshot.Id)},
               ds.{Public.DataSources.Name}                                  {nameof(DataSourceSnapshot.Name)},
               ds.{Public.DataSources.TypeId}                                {nameof(DataSourceSnapshot.TypeId)},
               ds.{Public.DataSources.ConnectionString}                      {nameof(DataSourceSnapshot.ConnectionString)},
   
               nc.{Public.NotificationChannels.Id}                           {nameof(NotificationChannelSnapshot.Id)},
               nc.{Public.NotificationChannels.Settings}                     {nameof(NotificationChannelSnapshot.Settings)},
               nc.{Public.NotificationChannels.DestinationId}                {nameof(NotificationChannelSnapshot.DestinationId)}
           FROM
               {Public.AlertTable} a
           INNER JOIN
               {Public.DataSourceTable} ds ON a.{Public.Alerts.DataSourceId} = {Public.DataSources.Id}
           INNER JOIN
               {Public.NotificationTable} nc ON {Public.NotificationChannels.Id} = {Public.Alerts.NotificationChannelId}
           WHERE
               a.{Public.Alerts.Status} = {AlertStatus.InActive.Value}
               AND
               a.{Public.Alerts.NextExecution} < NOW()
           ORDER BY
               a.{Public.Alerts.NextExecution}
           LIMIT @p_limit
           FOR UPDATE SKIP LOCKED;
        """;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var alertsToCheck = await connection
            .QueryAsync<AlertSnapshot, DataSourceSnapshot, NotificationChannelSnapshot, AlertSnapshot>(
                sql,
                map: (alert, dataSource, notificationChannel) =>
                {
                    alert.DataSource = dataSource;
                    alert.NotificationChannel = notificationChannel;
                    return alert;
                },
                splitOn: $"{nameof(DataSourceSnapshot.Id)}, {nameof(NotificationChannelSnapshot.Id)}",
                param: parameters,
                transaction: unitOfWork.Transaction);

        return alertsToCheck.Select(x => x.RestoreFromSnapshot(notificationChannelManager));
    }

    public async Task<IEnumerable<AlertEntity>> GetTriggeredAlertsAsync(int limit = 5, CancellationToken token = default)
    {
        var parameters = new { p_limit = limit };
        
        var sql = $"""
            SELECT 
                a.{Public.Alerts.Id}                             {nameof(AlertSnapshot.Id)},
                a.{Public.Alerts.Description}                    {nameof(AlertSnapshot.Description)},
                a.{Public.Alerts.Status}                         {nameof(AlertSnapshot.Status)},
                a.{Public.Alerts.RawQuery}                       {nameof(AlertSnapshot.RawQuery)},
                a.{Public.Alerts.DataSourceId}                   {nameof(AlertSnapshot.DataSourceId)},
                a.{Public.Alerts.NotificationChannelId}          {nameof(AlertSnapshot.NotificationChannelId)},
                a.{Public.Alerts.PreviousExecution}              {nameof(AlertSnapshot.PreviousExecution)},
                a.{Public.Alerts.NextExecution}                  {nameof(AlertSnapshot.NextExecution)},
                a.{Public.Alerts.WaitTimeBeforeAlertingInTicks}  {nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
                a.{Public.Alerts.RepeatIntervalInTicks}          {nameof(AlertSnapshot.RepeatIntervalInTicks)},
               
                ds.{Public.DataSources.Id}                     {nameof(DataSourceSnapshot.Id)},
                ds.{Public.DataSources.Name}                   {nameof(DataSourceSnapshot.Name)},
                ds.{Public.DataSources.TypeId}                 {nameof(DataSourceSnapshot.TypeId)},
                ds.{Public.DataSources.ConnectionString}       {nameof(DataSourceSnapshot.ConnectionString)},
               
                nc.{Public.NotificationChannels.Id}            {nameof(NotificationChannelSnapshot.Id)},
                nc.{Public.NotificationChannels.Settings}      {nameof(NotificationChannelSnapshot.Settings)},
                nc.{Public.NotificationChannels.DestinationId} {nameof(NotificationChannelSnapshot.DestinationId)}
           FROM
               {Public.AlertTable} a
           INNER JOIN
               {Public.DataSourceTable} ds ON a.{Public.Alerts.DataSourceId} = ds.{Public.DataSources.Id}
           INNER JOIN
               {Public.NotificationTable} nc ON nc.{Public.NotificationChannels.Id} = a.{Public.Alerts.NotificationChannelId}
           WHERE
               a.{Public.Alerts.Status} IN ({AlertStatus.Warning.Value}, {AlertStatus.Fire.Value}, {AlertStatus.Muted.Value})
               AND
               a.{Public.Alerts.NextExecution} < NOW()
           ORDER BY
               a.{Public.Alerts.NextExecution}
           LIMIT @p_limit
           FOR UPDATE SKIP LOCKED;
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var firedAlerts = await connection
            .QueryAsync<AlertSnapshot, DataSourceSnapshot, NotificationChannelSnapshot, AlertSnapshot>(
                sql,
                map: (alert, dataSource, notificationChannel) =>
                {
                    alert.DataSource = dataSource;
                    alert.NotificationChannel = notificationChannel;
                    return alert;
                },
                splitOn: $"{nameof(DataSourceSnapshot.Id)}, {nameof(NotificationChannelSnapshot.Id)}",
                param: parameters,
                transaction: unitOfWork.Transaction);

        return firedAlerts.Select(x => x.RestoreFromSnapshot(notificationChannelManager));
    }
}