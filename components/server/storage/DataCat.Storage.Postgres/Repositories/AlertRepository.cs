namespace DataCat.Storage.Postgres.Repositories;

public sealed class AlertRepository(
    UnitOfWork unitOfWork,
    IDbConnectionFactory<NpgsqlConnection> Factory,
    NotificationChannelManager notificationChannelManager)
    : IRepository<AlertEntity, Guid>, IAlertRepository
{
    public async Task<AlertEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_alert_id = id.ToString() };
        const string sql = AlertSql.Select.GetById;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QueryAsync<AlertSnapshot, NotificationChannelSnapshot, DataSourceSnapshot, AlertSnapshot>(
            sql,
            map: (alert, notification, dataSource) =>
            {
                alert.NotificationChannel = notification;
                alert.DataSource = dataSource;
                return alert;
            },
            splitOn: $"{nameof(NotificationChannelSnapshot.Id)}, {nameof(DataSourceSnapshot.Id)}",
            param: parameters,
            transaction: unitOfWork.Transaction);

        return result.FirstOrDefault()?.RestoreFromSnapshot(notificationChannelManager);
    }
    
    public async Task AddAsync(AlertEntity entity, CancellationToken token = default)
    {
        var alertSnapshot = entity.Save();
        
        const string sql = $@"
           INSERT INTO {Public.AlertTable} (
               {Public.Alerts.Id},
               {Public.Alerts.Description},
               {Public.Alerts.RawQuery},
               {Public.Alerts.Status},
               {Public.Alerts.DataSourceId},
               {Public.Alerts.NotificationChannelId},
               {Public.Alerts.PreviousExecution},
               {Public.Alerts.NextExecution},
               {Public.Alerts.RepeatIntervalInTicks},
               {Public.Alerts.WaitTimeBeforeAlertingInTicks}
           )
           VALUES (
               @{nameof(AlertSnapshot.Id)},
               @{nameof(AlertSnapshot.Description)},
               @{nameof(AlertSnapshot.RawQuery)},
               @{nameof(AlertSnapshot.Status)},
               @{nameof(AlertSnapshot.DataSourceId)},
               @{nameof(AlertSnapshot.NotificationChannelId)},
               @{nameof(AlertSnapshot.PreviousExecution)},
               @{nameof(AlertSnapshot.NextExecution)},
               @{nameof(AlertSnapshot.RepeatIntervalInTicks)},
               @{nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)}
           )";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, alertSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task<Page<AlertEntity>> SearchAsync(
        string? filter = null, 
        int page = 1, 
        int pageSize = 10, 
        CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        
        var totalQueryArguments = new { p_description = $"{filter}%" };
        const string totalCountSql = AlertSql.Select.SearchAlertsTotalCount;
        var totalCount = await connection.QuerySingleAsync<int>(totalCountSql, totalQueryArguments);
        
        var offset = (page - 1) * pageSize;
        var parameters = new { p_description = $"{filter}%", offset = offset, limit = pageSize };
        const string sql = AlertSql.Select.SearchAlerts;
        
        var result = await connection.QueryAsync<AlertSnapshot, NotificationChannelSnapshot, DataSourceSnapshot, AlertSnapshot>(
            sql,
            map: (alert, notification, dataSource) =>
            {
                alert.NotificationChannel = notification;
                alert.DataSource = dataSource;
                return alert;
            },
            splitOn: $"{nameof(NotificationChannelSnapshot.Id)}, {nameof(DataSourceSnapshot.Id)}",
            param: parameters,
            transaction: unitOfWork.Transaction);
        
        var items = result.Select(x => x.RestoreFromSnapshot(notificationChannelManager));
        return new Page<AlertEntity>(items, totalCount, page, pageSize);
    }

    public async Task UpdateAsync(AlertEntity entity, CancellationToken token = default)
    {
        var alertSnapshot = entity.Save();
        
        const string sql = $@"
           UPDATE {Public.AlertTable}
           SET 
               {Public.Alerts.Description}                    = @{nameof(AlertSnapshot.Description)},
               {Public.Alerts.Status}                         = @{nameof(AlertSnapshot.Status)},
               {Public.Alerts.RawQuery}                       = @{nameof(AlertSnapshot.RawQuery)},
               {Public.Alerts.DataSourceId}                   = @{nameof(AlertSnapshot.DataSourceId)},
               {Public.Alerts.NotificationChannelId}          = @{nameof(AlertSnapshot.NotificationChannelId)},
               {Public.Alerts.PreviousExecution}              = @{nameof(AlertSnapshot.PreviousExecution)},
               {Public.Alerts.NextExecution}                  = @{nameof(AlertSnapshot.NextExecution)},
               {Public.Alerts.RepeatIntervalInTicks}          = @{nameof(AlertSnapshot.RepeatIntervalInTicks)}, 
               {Public.Alerts.WaitTimeBeforeAlertingInTicks}  = @{nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)}
           WHERE {Public.Alerts.Id} = @{nameof(AlertSnapshot.Id)}
           ";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, alertSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_alert_id = id.ToString() };

        const string sql = $@"
           DELETE FROM {Public.AlertTable}
           WHERE {Public.Alerts.Id} = @p_alert_id
           ";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: unitOfWork.Transaction);
    }
}