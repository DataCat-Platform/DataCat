namespace DataCat.Storage.Postgres.Repositories;

public sealed class AlertRepository(
    UnitOfWork unitOfWork,
    IDbConnectionFactory<NpgsqlConnection> Factory)
    : IDefaultRepository<AlertEntity, Guid>
{
    public async Task<AlertEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { AlertId = id.ToString() };
        var sql = Sql.FindAlert;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QueryAsync<AlertSnapshot, NotificationChannelSnapshot, DataSourceSnapshot, AlertSnapshot>(
            sql,
            map: (alert, notification, dataSource) =>
            {
                alert.AlertNotificationChannel = notification;
                alert.AlertDataSource = dataSource;
                return alert;
            },
            splitOn: $"{Public.NotificationChannels.NotificationChannelId}, {Public.DataSources.DataSourceId}",
            param: parameters,
            transaction: unitOfWork.Transaction);

        return result.FirstOrDefault()?.RestoreFromSnapshot();
    }

    public async IAsyncEnumerable<AlertEntity> SearchAsync(
        string? filter = null, 
        int page = 1, 
        int pageSize = 10, 
        [EnumeratorCancellation] CancellationToken token = default)
    {
        var offset = (page - 1) * pageSize;
        var parameters = new { AlertDescription = $"%{filter}%", Page = offset, PageSize = pageSize };
        var sql = Sql.SearchAlerts;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QueryAsync<AlertSnapshot, NotificationChannelSnapshot, DataSourceSnapshot, AlertSnapshot>(
            sql,
            map: (alert, notification, dataSource) =>
            {
                alert.AlertNotificationChannel = notification;
                alert.AlertDataSource = dataSource;
                return alert;
            },
            splitOn: $"{Public.NotificationChannels.NotificationChannelId}, {Public.DataSources.DataSourceId}",
            param: parameters,
            transaction: unitOfWork.Transaction);
        
        foreach (var alert in result)
        {
            yield return alert.RestoreFromSnapshot();
        }
    }

    public async Task AddAsync(AlertEntity entity, CancellationToken token = default)
    {
        var alertSnapshot = entity.Save();
        
        var sql = $@"
           INSERT INTO {Public.AlertTable} (
               {Public.Alerts.AlertId},
               {Public.Alerts.AlertDescription},
               {Public.Alerts.AlertRawQuery},
               {Public.Alerts.AlertStatus},
               {Public.Alerts.AlertDataSourceId},
               {Public.Alerts.AlertNotificationChannelId},
               {Public.Alerts.AlertPreviousExecution},
               {Public.Alerts.AlertNextExecution},
               {Public.Alerts.AlertRepeatIntervalInTicks},
               {Public.Alerts.AlertWaitTimeBeforeAlertingInTicks}
           )
           VALUES (
               @AlertId,
               @AlertDescription,
               @AlertRawQuery,
               @AlertStatus,
               @AlertDataSourceId,
               @AlertNotificationChannelId,
               @AlertPreviousExecution,
               @AlertNextExecution,
               @AlertRepeatIntervalInTicks,
               @AlertWaitTimeBeforeAlertingInTicks
           )";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, alertSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task UpdateAsync(AlertEntity entity, CancellationToken token = default)
    {
        var alertSnapshot = entity.Save();
        
        var sql = $@"
           UPDATE {Public.AlertTable}
           SET 
               {Public.Alerts.AlertDescription}                    = @AlertDescription,
               {Public.Alerts.AlertStatus}                         = @AlertStatus,
               {Public.Alerts.AlertRawQuery}                       = @AlertRawQuery,
               {Public.Alerts.AlertDataSourceId}                   = @AlertDataSourceId,
               {Public.Alerts.AlertNotificationChannelId}          = @AlertNotificationChannelId,
               {Public.Alerts.AlertPreviousExecution}              = @AlertPreviousExecution,
               {Public.Alerts.AlertNextExecution}                  = @AlertNextExecution,
               {Public.Alerts.AlertRepeatIntervalInTicks}          = @AlertRepeatIntervalInTicks, 
               {Public.Alerts.AlertWaitTimeBeforeAlertingInTicks}  = @AlertWaitTimeBeforeAlertingInTicks
           WHERE {Public.Alerts.AlertId} = @AlertId
           ";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, alertSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { AlertId = id.ToString() };

        var sql = $@"
           DELETE FROM {Public.AlertTable}
           WHERE {Public.Alerts.AlertId} = @AlertId";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: unitOfWork.Transaction);
    }
}