namespace DataCat.Storage.Postgres.Repositories;

public sealed class PluginRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork)
    : IRepository<Plugin, Guid>, IPluginRepository
{
    public async Task<Plugin?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_plugin_id = id.ToString() };
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = $"""
            SELECT
                {Public.Plugins.Id}                  {nameof(PluginSnapshot.Id)},
                {Public.Plugins.Name}                {nameof(PluginSnapshot.Name)},
                {Public.Plugins.Version}             {nameof(PluginSnapshot.Version)},
                {Public.Plugins.Description}         {nameof(PluginSnapshot.Description)},
                {Public.Plugins.Author}              {nameof(PluginSnapshot.Author)},
                {Public.Plugins.IsEnabled}           {nameof(PluginSnapshot.IsEnabled)},
                {Public.Plugins.Settings}            {nameof(PluginSnapshot.Settings)},
                {Public.Plugins.CreatedAt}           {nameof(PluginSnapshot.CreatedAt)},
                {Public.Plugins.UpdatedAt}           {nameof(PluginSnapshot.UpdatedAt)}
            FROM {Public.PluginTable} 
            WHERE {Public.Plugins.Id} = @p_plugin_id
        """;
        var result = await connection.QueryAsync<PluginSnapshot>(sql, param: parameters, transaction: UnitOfWork.Transaction);

        var pluginSnapshot = result.FirstOrDefault();
        return pluginSnapshot?.RestoreFromSnapshot();
    }
    
    public async Task AddAsync(Plugin entity, CancellationToken token)
    {
        var pluginSnapshot = entity.Save();

        const string sql = $"""
            INSERT INTO {Public.PluginTable} (
                {Public.Plugins.Id},
                {Public.Plugins.Name},
                {Public.Plugins.Version},
                {Public.Plugins.Description},
                {Public.Plugins.Author},
                {Public.Plugins.IsEnabled},
                {Public.Plugins.Settings},
                {Public.Plugins.CreatedAt},
                {Public.Plugins.UpdatedAt}
            )
            VALUES (
                @{nameof(PluginSnapshot.Id)}, 
                @{nameof(PluginSnapshot.Name)},
                @{nameof(PluginSnapshot.Version)},
                @{nameof(PluginSnapshot.Description)},
                @{nameof(PluginSnapshot.Author)},
                @{nameof(PluginSnapshot.IsEnabled)},
                @{nameof(PluginSnapshot.Settings)},
                @{nameof(PluginSnapshot.CreatedAt)},
                @{nameof(PluginSnapshot.UpdatedAt)}
            )
        """;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, pluginSnapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task<Page<Plugin>> SearchAsync(
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
            ["name"] = $"{Public.Plugins.Name}",
            ["isEnabled"] = $"{Public.Plugins.IsEnabled}"
        };

        var countSql = new StringBuilder();
        countSql.AppendLine(PluginSql.Select.SearchPluginsTotalCount);
        countSql.BuildQuery(parameters, filters, columnMappings);

        var countSqlString = countSql.ToString();
        var totalCount = await connection.QuerySingleAsync<int>(
            countSqlString,
            parameters,
            transaction: UnitOfWork.Transaction);

        var dataSql = new StringBuilder();
        dataSql.AppendLine(PluginSql.Select.SearchPlugins);
        dataSql
            .BuildQuery(parameters, filters, columnMappings)
            .ApplyOrderBy(filters.Sort ?? new Sort(FieldName: "name"), columnMappings)
            .ApplyPagination();

        var dataSqlString = dataSql.ToString();

        var result = await connection.QueryAsync<PluginSnapshot>(
            dataSqlString, 
            param: parameters, 
            transaction: UnitOfWork.Transaction);

        var items = result.Select(x => x.RestoreFromSnapshot());
        return new Page<Plugin>(items, totalCount, page, pageSize);
    }

    public async Task UpdateAsync(Plugin entity, CancellationToken token = default)
    {
        var pluginSnapshot = entity.Save();

        const string sql = $"""
            UPDATE {Public.PluginTable} 
            SET 
                {Public.Plugins.Name}        = @{nameof(PluginSnapshot.Name)}, 
                {Public.Plugins.Version}     = @{nameof(PluginSnapshot.Version)}, 
                {Public.Plugins.Description} = @{nameof(PluginSnapshot.Description)}, 
                {Public.Plugins.Author}      = @{nameof(PluginSnapshot.Author)}, 
                {Public.Plugins.IsEnabled}   = @{nameof(PluginSnapshot.IsEnabled)}, 
                {Public.Plugins.Settings}    = @{nameof(PluginSnapshot.Settings)}, 
                {Public.Plugins.UpdatedAt}   = @{nameof(PluginSnapshot.UpdatedAt)}
            WHERE {Public.Plugins.Id} = @{nameof(PluginSnapshot.Id)}
        """;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, pluginSnapshot, transaction: UnitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_plugin_id = id.ToString() };

        const string sql = $"""
            DELETE FROM {Public.PluginTable} 
            WHERE {Public.Plugins.Id} = @p_plugin_id
        """;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, param: parameters, transaction: UnitOfWork.Transaction);
    }
    
    public async Task<bool> ToggleStatusAsync(Guid id, bool isEnabled, CancellationToken token = default)
    {
        var parameters = new
        {
            p_plugin_id = id.ToString(), 
            p_is_enabled = isEnabled, 
            p_updated_at = DateTime.UtcNow
        };
        
        const string sql = $"""
            UPDATE {Public.PluginTable} 
            SET 
                {Public.Plugins.IsEnabled} = @p_is_enabled,
                {Public.Plugins.UpdatedAt} = @p_updated_at
            WHERE {Public.Plugins.Id} = @p_plugin_id
        """;
        
        var command = new CommandDefinition(sql, parameters, transaction: UnitOfWork.Transaction);
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var affectedRows = await connection.ExecuteAsync(command);
        return affectedRows > 0;
    }
}