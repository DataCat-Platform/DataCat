namespace DataCat.Storage.Postgres.Repositories;

public sealed class DashboardRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork)
    : IRepository<Dashboard, Guid>, IDashboardRepository
{
    public async Task<Dashboard?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_dashboard_id = id.ToString() };
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        const string sql = DashboardSql.Select.FindDashboard;
        
        var dashboardDictionary = new Dictionary<string, DashboardSnapshot>();
        await connection.QueryAsync<DashboardSnapshot, UserSnapshot, PanelSnapshot?, UserSnapshot?, DataSourceSnapshot, DataSourceTypeSnapshot, DashboardSnapshot>(
            sql,
            map: (dashboard, userOwner, panel, sharedUser, dataSource, dataSourceType) =>
            {
                if (!dashboardDictionary.TryGetValue(dashboard.Id, out var existingDashboard))
                {
                    dashboard.Owner = userOwner;
                    dashboard.Panels = new List<PanelSnapshot>();
                    dashboard.SharedWith = new List<UserSnapshot>();
                    dashboardDictionary.Add(dashboard.Id, dashboard);
                    existingDashboard = dashboard;
                }

                if (panel is not null && existingDashboard.Panels.All(p => p.Id != panel.Id))
                {
                    dataSource.DataSourceType = dataSourceType;
                    panel.DataSource = dataSource;
                    existingDashboard.Panels.Add(panel);
                }

                if (sharedUser is not null && existingDashboard.SharedWith.All(u => u.UserId != sharedUser.UserId))
                {
                    existingDashboard.SharedWith.Add(sharedUser);
                }

                return existingDashboard;
            },
            splitOn: $"{nameof(UserSnapshot.UserId)}, {nameof(PanelSnapshot.Id)}, {nameof(UserSnapshot.UserId)}, {nameof(DataSourceSnapshot.Id)}",
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
                {Public.Dashboards.OwnerId},
                {Public.Dashboards.NamespaceId},
                {Public.Dashboards.CreatedAt}, 
                {Public.Dashboards.UpdatedAt},
                {Public.Dashboards.Tags}
            )
            VALUES 
                (@{nameof(DashboardSnapshot.Id)}, 
                 @{nameof(DashboardSnapshot.Name)}, 
                 @{nameof(DashboardSnapshot.Description)}, 
                 @{nameof(DashboardSnapshot.OwnerId)},
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
        parameters.Add("offset", offset);
        parameters.Add("limit", pageSize);

        var columnMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = $"dashboard.{Public.Dashboards.Id}",
            ["name"] = $"dashboard.{Public.Dashboards.Name}",
            ["ownerId"] = $"dashboard.{Public.Dashboards.OwnerId}",
            ["namespaceId"] = $"dashboard.{Public.Dashboards.NamespaceId}",
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
            .ApplyOrderBy(filters.Sort ?? new Sort(FieldName: $"dashboard.{Public.Dashboards.Id}"), columnMappings)
            .ApplyPagination();
        
        var dataSqlString = dataSql.ToString();

        var dashboardDictionary = new Dictionary<string, DashboardSnapshot>();

        await connection
            .QueryAsync<DashboardSnapshot, UserSnapshot, PanelSnapshot?, UserSnapshot?, DataSourceSnapshot, DataSourceTypeSnapshot, DashboardSnapshot>(
                dataSqlString,
                map: (dashboard, userOwner, panel, sharedUser, dataSource, dataSourceType) =>
                {
                    if (!dashboardDictionary.TryGetValue(dashboard.Id, out var existingDashboard))
                    {
                        dashboard.Owner = userOwner;
                        dashboard.Panels = new List<PanelSnapshot>();
                        dashboard.SharedWith = new List<UserSnapshot>();
                        dashboardDictionary[dashboard.Id] = dashboard;
                        existingDashboard = dashboard;
                    }

                    if (panel is not null && existingDashboard.Panels.All(p => p.Id != panel.Id))
                    {
                        dataSource.DataSourceType = dataSourceType;
                        panel.DataSource = dataSource;
                        existingDashboard.Panels.Add(panel);
                    }

                    if (sharedUser is not null && existingDashboard.SharedWith.All(u => u.UserId != sharedUser.UserId))
                    {
                        existingDashboard.SharedWith.Add(sharedUser);
                    }

                    return existingDashboard;
                },
                splitOn: $"{nameof(UserSnapshot.UserId)}, {nameof(PanelSnapshot.Id)}, {nameof(UserSnapshot.UserId)}, {nameof(DataSourceSnapshot.Id)}",
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
            Name = x.Name,
            Description = x.Description,
            OwnerId = Guid.Parse(x.OwnerId),
            UpdatedAt = x.UpdatedAt
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
    
    public async Task AddUserToDashboard(User user, Dashboard dashboard, CancellationToken token = default)
    {
        var parameters = new
        {
            dashboard_id = dashboard.Id.ToString(),
            user_id = user.Id.ToString(),
        };
        const string sql = $"""
            INSERT INTO {Public.DashboardUserLinkTable} (
               {Public.Dashboards.Id},
               {Public.Users.Id}
            ) 
            VALUES (
               @dashboard_id,
               @user_id
            );
        """;
        
        var command = new CommandDefinition(sql, parameters, transaction: UnitOfWork.Transaction);
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        await connection.ExecuteAsync(command);
    }
}