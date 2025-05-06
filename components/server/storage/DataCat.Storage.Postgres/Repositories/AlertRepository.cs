namespace DataCat.Storage.Postgres.Repositories;

public sealed class AlertRepository(
    UnitOfWork unitOfWork,
    IDbConnectionFactory<NpgsqlConnection> Factory,
    NotificationChannelManager notificationChannelManager)
    : IRepository<Alert, Guid>, IAlertRepository
{
    public async Task<Alert?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_alert_id = id.ToString() };
        const string sql = AlertSql.Select.GetById;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        
        var alertDictionary = new Dictionary<string, AlertSnapshot>();

        await connection.QueryAsync<
            AlertSnapshot,
            NotificationChannelGroupSnapshot,
            NotificationChannelSnapshot,
            DataSourceSnapshot,
            DataSourceTypeSnapshot,
            AlertSnapshot>(
            sql,
            map: (alert, group, channel, dataSource, dataSourceType) =>
            {
                if (!alertDictionary.TryGetValue(alert.Id, out var existingAlert))
                {
                    dataSource.DataSourceType = dataSourceType;
                    alert.DataSource = dataSource;

                    alert.NotificationChannelGroup = group;

                    alertDictionary[alert.Id] = alert;
                    existingAlert = alert;
                }

                if (existingAlert.NotificationChannelGroup.Channels.All(c => c.Id != channel.Id))
                {
                    existingAlert.NotificationChannelGroup.Channels.Add(channel);
                }

                return existingAlert;
            },
            splitOn: $"{nameof(NotificationChannelGroupSnapshot.Id)}, {nameof(NotificationChannelSnapshot.Id)}, {nameof(DataSourceSnapshot.Id)}, {nameof(DataSourceTypeSnapshot.Id)}",
            param: parameters,
            transaction: unitOfWork.Transaction);

        return alertDictionary.Values.FirstOrDefault()?.RestoreFromSnapshot(notificationChannelManager);
    }
    
    public async Task AddAsync(Alert entity, CancellationToken token = default)
    {
        var alertSnapshot = entity.Save();
        
        const string sql = $@"
           INSERT INTO {Public.AlertTable} (
               {Public.Alerts.Id},
               {Public.Alerts.Description},
               {Public.Alerts.RawQuery},
               {Public.Alerts.Status},
               {Public.Alerts.DataSourceId},
               {Public.Alerts.NotificationChannelGroupId},
               {Public.Alerts.PreviousExecution},
               {Public.Alerts.NextExecution},
               {Public.Alerts.RepeatIntervalInTicks},
               {Public.Alerts.WaitTimeBeforeAlertingInTicks},
               {Public.Alerts.Tags}
           )
           VALUES (
               @{nameof(AlertSnapshot.Id)},
               @{nameof(AlertSnapshot.Description)},
               @{nameof(AlertSnapshot.ConditionQuery)},
               @{nameof(AlertSnapshot.Status)},
               @{nameof(AlertSnapshot.DataSourceId)},
               @{nameof(AlertSnapshot.NotificationChannelGroupId)},
               @{nameof(AlertSnapshot.PreviousExecution)},
               @{nameof(AlertSnapshot.NextExecution)},
               @{nameof(AlertSnapshot.RepeatIntervalInTicks)},
               @{nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
               @{nameof(AlertSnapshot.Tags)}
           )";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, alertSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task<Page<Alert>> SearchAsync(
        SearchFilters filters, 
        int page = 1, 
        int pageSize = 10, 
        CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var parameters = new DynamicParameters();
        
        var offset = (page - 1) * pageSize;
        parameters.Add("offset", offset);
        parameters.Add("limit", pageSize);
        
        var columnMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = $"alerts.{Public.Alerts.Id}",
            ["rawQuery"] = $"alert.{Public.Alerts.RawQuery}",
            ["tags"] = $"alert.{Public.Alerts.Tags}",
        };
        
        var countSql = new StringBuilder();
        countSql.AppendLine(AlertSql.Select.SearchAlertsTotalCount);
        
        countSql.BuildQuery(parameters, filters, columnMappings);
        
        var countSqlString = countSql.ToString();
        
        var totalCount = await connection.QuerySingleAsync<int>(
            countSqlString,
            parameters,
            transaction: unitOfWork.Transaction);
        
        var dataSql = new StringBuilder();
        dataSql.AppendLine(AlertSql.Select.SearchAlerts);
        dataSql
            .BuildQuery(parameters, filters, columnMappings)
            .ApplyOrderBy(filters.Sort ?? new Sort(FieldName: "id"), columnMappings)
            .ApplyPagination();
        
        var dataSqlString = dataSql.ToString();
        
        var alertDictionary = new Dictionary<string, AlertSnapshot>();

        await connection.QueryAsync<
            AlertSnapshot,
            NotificationChannelGroupSnapshot,
            NotificationChannelSnapshot,
            DataSourceSnapshot,
            DataSourceTypeSnapshot,
            AlertSnapshot>(
            dataSqlString,
            map: (alert, group, channel, dataSource, dataSourceType) =>
            {
                if (!alertDictionary.TryGetValue(alert.Id, out var existingAlert))
                {
                    dataSource.DataSourceType = dataSourceType;
                    alert.DataSource = dataSource;
                    alert.NotificationChannelGroup = group;
                    alertDictionary[alert.Id] = alert;
                    existingAlert = alert;
                }

                if (existingAlert.NotificationChannelGroup.Channels.All(c => c.Id != channel.Id))
                {
                    existingAlert.NotificationChannelGroup.Channels.Add(channel);
                }

                return existingAlert;
            },
            splitOn: $"{nameof(NotificationChannelGroupSnapshot.Id)}, {nameof(NotificationChannelSnapshot.Id)}, {nameof(DataSourceSnapshot.Id)}, {nameof(DataSourceTypeSnapshot.Id)}",
            param: parameters,
            transaction: unitOfWork.Transaction);

        var items = alertDictionary
            .Select(x => x.Value)
            .Select(x => x.RestoreFromSnapshot(notificationChannelManager));

        return new Page<Alert>(items, totalCount, page, pageSize);
    }

    public async Task UpdateAsync(Alert entity, CancellationToken token = default)
    {
        var alertSnapshot = entity.Save();
        
        const string sql = $@"
           UPDATE {Public.AlertTable}
           SET 
               {Public.Alerts.Description}                    = @{nameof(AlertSnapshot.Description)},
               {Public.Alerts.Status}                         = @{nameof(AlertSnapshot.Status)},
               {Public.Alerts.RawQuery}                       = @{nameof(AlertSnapshot.ConditionQuery)},
               {Public.Alerts.DataSourceId}                   = @{nameof(AlertSnapshot.DataSourceId)},
               {Public.Alerts.NotificationChannelGroupId}     = @{nameof(AlertSnapshot.NotificationChannelGroupId)},
               {Public.Alerts.PreviousExecution}              = @{nameof(AlertSnapshot.PreviousExecution)},
               {Public.Alerts.NextExecution}                  = @{nameof(AlertSnapshot.NextExecution)},
               {Public.Alerts.RepeatIntervalInTicks}          = @{nameof(AlertSnapshot.RepeatIntervalInTicks)}, 
               {Public.Alerts.WaitTimeBeforeAlertingInTicks}  = @{nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
               {Public.Alerts.Tags}                           = @{nameof(AlertSnapshot.Tags)}
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