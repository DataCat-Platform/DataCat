namespace DataCat.Server.Postgres.Repositories;

public sealed class PluginRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory)
    : IDefaultRepository<PluginEntity, Guid>
{
    public async Task<PluginEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { PluginId = id.ToString() };
        var connection = await Factory.CreateConnectionAsync(token);

        var sql = $"SELECT * FROM {Public.PluginTable} WHERE {Public.Plugins.PluginId} = @PluginId";
        var result = await connection.QueryAsync<PluginSnapshot>(sql, param: parameters);

        var pluginSnapshot = result.FirstOrDefault();
        return pluginSnapshot?.RestoreFromSnapshot();
    }

    public async Task<IEnumerable<PluginEntity>> SearchAsync(string? filter = null, int page = 1, int pageSize = 1, CancellationToken token = default)
    {
        var connection = await Factory.CreateConnectionAsync(token);
        var offset = (page - 1) * pageSize;
        var sql = $"SELECT * FROM {Public.PluginTable} ";

        if (!string.IsNullOrEmpty(filter))
        {
            sql += $"WHERE {Public.Plugins.Name} LIKE @Filter ";
        }

        sql += "LIMIT @PageSize OFFSET @Offset";

        var result = await connection.QueryAsync<PluginSnapshot>(sql, new { Filter = $"%{filter}%", PageSize = pageSize, Offset = offset });
        return result.Select(PluginEntitySnapshotMapper.RestoreFromSnapshot);
    }

    public async Task AddAsync(PluginEntity entity, CancellationToken token)
    {
        var pluginSnapshot = entity.Save();

        var sql = 
            $"""
             INSERT INTO plugin (
                 {Public.Plugins.PluginId},
                 {Public.Plugins.Name},
                 {Public.Plugins.Version},
                 {Public.Plugins.Description},
                 {Public.Plugins.Author},
                 {Public.Plugins.IsEnabled},
                 {Public.Plugins.Settings},
                 {Public.Plugins.CreatedAt},
                 {Public.Plugins.UpdatedAt}
             )
             VALUES (@PluginId, @Name, @Version, @Description, @Author, @IsEnabled, @Settings, @CreatedAt, @UpdatedAt)
             """;
        
        var command = new CommandDefinition(sql, pluginSnapshot);
        var connection = await Factory.CreateConnectionAsync(token);

        await connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(PluginEntity entity, CancellationToken token = default)
    {
        var pluginSnapshot = entity.Save();

        var sql = 
            $"""
             UPDATE {Public.PluginTable} 
             SET 
                 {Public.Plugins.Name} = @Name, 
                 {Public.Plugins.Version} = @Version, 
                 {Public.Plugins.Description} = @Description, 
                 {Public.Plugins.Author} = @Author, 
                 {Public.Plugins.IsEnabled} = @IsEnabled, 
                 {Public.Plugins.Settings} = @Settings, 
                 {Public.Plugins.UpdatedAt} = @UpdatedAt
             WHERE {Public.Plugins.PluginId} = @PluginId
             """;
        
        var command = new CommandDefinition(sql, pluginSnapshot);
        var connection = await Factory.CreateConnectionAsync(token);

        await connection.ExecuteAsync(command);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { PluginId = id.ToString() };

        var sql = $"DELETE FROM {Public.PluginTable} WHERE {Public.Plugins.PluginId} = @PluginId";
        
        var connection = await Factory.CreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, param: parameters);
    }
}