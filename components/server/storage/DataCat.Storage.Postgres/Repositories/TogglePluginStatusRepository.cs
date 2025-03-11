namespace DataCat.Storage.Postgres.Repositories;

public sealed class TogglePluginStatusRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork)
    : ITogglePluginStatusRepository
{
    public async Task<bool> ToggleStatusAsync(Guid id, bool isEnabled, CancellationToken token = default)
    {
        var parameters = new { PluginId = id.ToString(), IsEnabled = isEnabled, UpdatedAt = DateTime.UtcNow };
        var sql = 
            $"""
             UPDATE {Public.PluginTable} 
             SET 
                 {Public.Plugins.PluginIsEnabled} = @PluginIsEnabled,
                 {Public.Plugins.PluginUpdatedAt} = @PluginUpdatedAt
             WHERE {Public.Plugins.PluginId} = @PluginId
             """;
        
        var command = new CommandDefinition(sql, parameters, transaction: UnitOfWork.Transaction);
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var affectedRows = await connection.ExecuteAsync(command);
        return affectedRows > 0;
    }
}