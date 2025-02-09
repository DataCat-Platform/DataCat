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

    public async IAsyncEnumerable<PluginEntity> SearchAsync(
        string? filter = null, 
        int page = 1, 
        int pageSize = 1, 
        [EnumeratorCancellation] CancellationToken token = default)
    {
        var connection = await Factory.CreateConnectionAsync(token);
        var offset = (page - 1) * pageSize;
        var sql = $"SELECT * FROM {Public.PluginTable} ";

        if (!string.IsNullOrEmpty(filter))
        {
            sql += $"WHERE {Public.Plugins.PluginName} LIKE @Filter ";
        }

        sql += "LIMIT @PageSize OFFSET @Offset";
        
        await using var reader = await connection.ExecuteReaderAsync(sql, new { Filter = $"{filter}%", PageSize = pageSize, Offset = offset });
        
        while (await reader.ReadAsync(token))
        {
            var snapshot = reader.ReadPlugin();
            yield return snapshot.RestoreFromSnapshot();
        }
    }

    public async Task AddAsync(PluginEntity entity, CancellationToken token)
    {
        var pluginSnapshot = entity.Save();

        var sql = 
            $"""
             INSERT INTO {Public.PluginTable} (
                 {Public.Plugins.PluginId},
                 {Public.Plugins.PluginName},
                 {Public.Plugins.PluginVersion},
                 {Public.Plugins.PluginDescription},
                 {Public.Plugins.PluginAuthor},
                 {Public.Plugins.PluginIsEnabled},
                 {Public.Plugins.PluginSettings},
                 {Public.Plugins.PluginCreatedAt},
                 {Public.Plugins.PluginUpdatedAt}
             )
             VALUES (@PluginId, @PluginName, @PluginVersion, @PluginDescription, @PluginAuthor, @PluginIsEnabled, @PluginSettings, @PluginCreatedAt, @PluginUpdatedAt)
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
                 {Public.Plugins.PluginName} = @PluginName, 
                 {Public.Plugins.PluginVersion} = @PluginVersion, 
                 {Public.Plugins.PluginDescription} = @PluginDescription, 
                 {Public.Plugins.PluginAuthor} = @PluginAuthor, 
                 {Public.Plugins.PluginIsEnabled} = @PluginIsEnabled, 
                 {Public.Plugins.PluginSettings} = @PluginSettings, 
                 {Public.Plugins.PluginUpdatedAt} = @PluginUpdatedAt
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