namespace DataCat.Storage.Postgres.Repositories;

public sealed class DashboardRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork,
    NamespaceContext NamespaceContext)
    : IRepository<Dashboard, Guid>, IDashboardRepository
{
    public async Task<Dashboard?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_dashboard_id = id.ToString(), p_namespace_id = NamespaceContext.NamespaceId };
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        const string sql = DashboardSql.Select.FindDashboard;
        
        var dashboardDictionary = new Dictionary<string, DashboardSnapshot>();
        await connection.QueryAsync<
            DashboardSnapshot,
            PanelSnapshot?,
            DataSourceSnapshot?,
            DataSourceTypeSnapshot?,
            DashboardSnapshot>(
            sql,
            map: (dashboard, panel, dataSource, dataSourceType) =>
            {
                if (!dashboardDictionary.TryGetValue(dashboard.Id, out var existingDashboard))
                {
                    dashboard.Panels = new List<PanelSnapshot>();
                    dashboardDictionary.Add(dashboard.Id, dashboard);
                    existingDashboard = dashboard;
                }

                if (panel is not null && existingDashboard.Panels.All(p => p.Id != panel.Id))
                {
                    dataSource!.DataSourceType = dataSourceType!;
                    panel.DataSource = dataSource;
                    existingDashboard.Panels.Add(panel);
                }

                return existingDashboard;
            },
            splitOn: $"{nameof(PanelSnapshot.Id)}, {nameof(DataSourceSnapshot.Id)}, {nameof(DataSourceTypeSnapshot.Id)}",
            param: parameters, 
            transaction: UnitOfWork.Transaction);

        var dashboardSnapshot = dashboardDictionary.FirstOrDefault().Value;
        return dashboardSnapshot?.RestoreFromSnapshot();
    }
    
    public async Task AddAsync(Dashboard entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();
        const string sql = $"""
            INSERT INTO {Public.DashboardTable}(
                {Public.Dashboards.Id}, 
                {Public.Dashboards.Name}, 
                {Public.Dashboards.Description},
                {Public.Dashboards.NamespaceId},
                {Public.Dashboards.CreatedAt}, 
                {Public.Dashboards.UpdatedAt},
                {Public.Dashboards.Tags}
            )
            VALUES 
                (@{nameof(DashboardSnapshot.Id)}, 
                 @{nameof(DashboardSnapshot.Name)}, 
                 @{nameof(DashboardSnapshot.Description)}, 
                 @{nameof(DashboardSnapshot.NamespaceId)}, 
                 @{nameof(DashboardSnapshot.CreatedAt)}, 
                 @{nameof(DashboardSnapshot.UpdatedAt)},
                 @{nameof(DashboardSnapshot.Tags)}
            );
            """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task<Page<Dashboard>> SearchAsync(
        SearchFilters filters,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var parameters = new DynamicParameters();

        var offset = (page - 1) * pageSize;
        parameters.Add("p_namespace_id", NamespaceContext.NamespaceId);
        parameters.Add("offset", offset);
        parameters.Add("limit", pageSize);

        var columnMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = $"dashboard.{Public.Dashboards.Id}",
            ["name"] = $"dashboard.{Public.Dashboards.Name}",
            ["tags"] = $"dashboard.{Public.Dashboards.Tags}",
        };

        var countSql = new StringBuilder();
        countSql.AppendLine(DashboardSql.Select.SearchDashboardsTotalCount);
        countSql.BuildQuery(parameters, filters, columnMappings);
        
        var countSqlString = countSql.ToString();
        
        var totalCount = await connection.QuerySingleAsync<int>(
            countSqlString,
            parameters,
            transaction: UnitOfWork.Transaction);

        var dataSql = new StringBuilder();
        dataSql.AppendLine(DashboardSql.Select.SearchDashboards);
        dataSql
            .BuildQuery(parameters, filters, columnMappings)
            .ApplyOrderBy(filters.Sort ?? new Sort(FieldName: "id"), columnMappings)
            .ApplyPagination();
        
        var dataSqlString = dataSql.ToString();

        var dashboardDictionary = new Dictionary<string, DashboardSnapshot>();

        await connection.QueryAsync<
                DashboardSnapshot, 
                PanelSnapshot?, 
                DataSourceSnapshot?, 
                DataSourceTypeSnapshot?, 
                DashboardSnapshot>(
                dataSqlString,
                map: (dashboard, panel, dataSource, dataSourceType) =>
                {
                    if (!dashboardDictionary.TryGetValue(dashboard.Id, out var existingDashboard))
                    {
                        dashboard.Panels = new List<PanelSnapshot>();
                        dashboardDictionary[dashboard.Id] = dashboard;
                        existingDashboard = dashboard;
                    }

                    if (panel is not null && existingDashboard.Panels.All(p => p.Id != panel.Id))
                    {
                        dataSource!.DataSourceType = dataSourceType!;
                        panel.DataSource = dataSource;
                        existingDashboard.Panels.Add(panel);
                    }

                    return existingDashboard;
                },
                splitOn: $"{nameof(PanelSnapshot.Id)}, {nameof(DataSourceSnapshot.Id)}, {nameof(DataSourceTypeSnapshot.Id)}",
                param: parameters,
                transaction: UnitOfWork.Transaction);

        var items = dashboardDictionary.Values.Select(x => x.RestoreFromSnapshot());
        return new Page<Dashboard>(items, totalCount, page, pageSize);
    }

    public async Task<IReadOnlyCollection<DashboardResponse>> GetDashboardsByNamespaceId(Guid id, CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        
        var parameters = new { p_namespace_id = id.ToString() };
        
        var result = await connection.QueryAsync<DashboardSnapshot>(
            sql: DashboardSql.Select.GetDashboardsByNamespaceId,
            param: parameters,
            transaction: UnitOfWork.Transaction);

        return result.Select(x => new DashboardResponse()
        {
            Id = Guid.Parse(x.Id),
            NamespaceId = Guid.Parse(x.NamespaceId),
            Name = x.Name,
            Description = x.Description,
            UpdatedAt = x.UpdatedAt,
            CreatedAt = x.CreatedAt,
            Tags = x.Tags.Select(tag => tag.Value).OrderBy(value => value).ToList()  
        }).ToList();
    }

    public async Task UpdateAsync(Dashboard entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();
        const string sql = $"""
            UPDATE {Public.DashboardTable}
            SET 
                {Public.Dashboards.Name}         = @{nameof(DashboardSnapshot.Name)},
                {Public.Dashboards.Description}  = @{nameof(DashboardSnapshot.Description)},
                {Public.Dashboards.UpdatedAt}    = @{nameof(DashboardSnapshot.UpdatedAt)},
                {Public.Dashboards.Tags}         = @{nameof(DashboardSnapshot.Tags)}
            WHERE {Public.Dashboards.Id} = @{nameof(DashboardSnapshot.Id)}
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_dashboard_id = id.ToString() };
        
        const string sql = $"""
            DELETE FROM {Public.DashboardTable} 
            WHERE {Public.Dashboards.Id} = @p_dashboard_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: UnitOfWork.Transaction);
    }
}