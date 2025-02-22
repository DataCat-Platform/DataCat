namespace DataCat.Storage.Postgres.Repositories;

public sealed class DashboardRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory)
    : IDefaultRepository<DashboardEntity, Guid>
{
    public async Task<DashboardEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { DashboardId = id.ToString() };
        var connection = await Factory.CreateConnectionAsync(token);
        var sql = Sql.FindDashboard;
        
        var dashboardDictionary = new Dictionary<string, DashboardSnapshot>();
        await connection
            .QueryAsync<DashboardSnapshot, UserSnapshot, PanelSnapshot?, UserSnapshot?, DataSourceSnapshot, DashboardSnapshot>(
                sql,
                map: MapFunctions.MapDashboard(dashboardDictionary),
                splitOn: $"{Public.Users.UserId}, {Public.Panels.PanelId}, {Public.Users.UserId}, {Public.DataSources.DataSourceId}",
                param: parameters);

        var dashboardSnapshot = dashboardDictionary.FirstOrDefault().Value;
        return dashboardSnapshot?.RestoreFromSnapshot();
    }

    public async IAsyncEnumerable<DashboardEntity> SearchAsync(
        string? filter = null, 
        int page = 1,
        int pageSize = 10, 
        [EnumeratorCancellation] CancellationToken token = default)
    {
        var offset = (page - 1) * pageSize;
        var parameters = new { Name = $"{filter}%", Page = offset, PageSize = pageSize };
        var connection = await Factory.CreateConnectionAsync(token);
        var sql = Sql.SearchDashboards;
        
        var dashboardDictionary = new Dictionary<string, DashboardSnapshot>();
        
        await connection
                .QueryAsync<DashboardSnapshot, UserSnapshot, PanelSnapshot?, UserSnapshot?, DataSourceSnapshot, DashboardSnapshot>(
                    sql,
                    map: MapFunctions.MapDashboard(dashboardDictionary),
                    splitOn: $"{Public.Users.UserId}, {Public.Panels.PanelId}, {Public.Users.UserId}, {Public.DataSources.DataSourceId}",
                    param: parameters);

        foreach (var dashboard in dashboardDictionary.Values)
        {
            yield return dashboard.RestoreFromSnapshot();
        }
    }

    public async Task AddAsync(DashboardEntity entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();
        var sql = $"""
            INSERT INTO {DashboardSnapshot.DashboardTable}(
                {Public.Dashboards.DashboardId}, 
                {Public.Dashboards.DashboardName}, 
                {Public.Dashboards.DashboardDescription}, 
                {Public.Dashboards.DashboardOwnerId}, 
                {Public.Dashboards.DashboardCreatedAt}, 
                {Public.Dashboards.DashboardUpdatedAt})
            VALUES 
                (@DashboardId, 
                 @DashboardName, 
                 @DashboardDescription, 
                 @OwnerId, 
                 @DashboardCreatedAt, 
                 @DashboardUpdatedAt);
            """;

        var connection = await Factory.CreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot);
    }

    public async Task UpdateAsync(DashboardEntity entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();
        var sql = $"""
            UPDATE {DashboardSnapshot.DashboardTable}
            SET 
                {Public.Dashboards.DashboardName} = @DashboardName,
                {Public.Dashboards.DashboardDescription} = @DashboardDescription,
                {Public.Dashboards.DashboardUpdatedAt} = @DashboardUpdatedAt
            WHERE {Public.Dashboards.DashboardId} = @DashboardId
        """;

        var connection = await Factory.CreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var sql = $"""
            DELETE FROM {DashboardSnapshot.DashboardTable} 
            WHERE {Public.Dashboards.DashboardId} = @DashboardId
            """;

        var connection = await Factory.CreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, new { DashboardId = id.ToString() });
    }
}