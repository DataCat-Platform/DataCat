namespace DataCat.Server.Postgres.Repositories;

public class PanelRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory)
    : IDefaultRepository<PanelEntity, Guid>
{
    public async Task<PanelEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var sql = $"SELECT * FROM {Public.PanelTable} WHERE {Public.Panels.PanelId} = @PanelId";
        var parameters = new { PanelId = id };

        var connection = await Factory.CreateConnectionAsync(token);
        var result = await connection.QuerySingleOrDefaultAsync<PanelSnapshot>(sql, parameters);

        return result?.RestoreFromSnapshot();
    }

    public async IAsyncEnumerable<PanelEntity> SearchAsync(
        string? filter = null, 
        int page = 1, 
        int pageSize = 10, 
        [EnumeratorCancellation] CancellationToken token = default)
    {
        var offset = (page - 1) * pageSize;
        var sql = $"SELECT * FROM {Public.PanelTable} ";

        if (!string.IsNullOrEmpty(filter))
        {
            sql += $"WHERE {Public.Panels.PanelTitle} LIKE @Filter ";
        }

        sql += "LIMIT @PageSize OFFSET @Offset";

        var parameters = new { Filter = $"{filter}%", PageSize = pageSize, Offset = offset };

        var connection = await Factory.CreateConnectionAsync(token);
        await using var reader = await connection.ExecuteReaderAsync(sql, parameters);

        while (await reader.ReadAsync(token))
        {
            var snapshot = reader.ReadPanel();
            yield return snapshot.RestoreFromSnapshot();
        }
    }

    public async Task AddAsync(PanelEntity entity, CancellationToken token = default)
    {
        var panelSnapshot = entity.Save();

        var sql = $"""
           INSERT INTO {Public.PanelTable} (
               {Public.Panels.PanelId},
               {Public.Panels.PanelTitle},
               {Public.Panels.PanelType},
               {Public.Panels.PanelRawQuery},
               {Public.Panels.PanelDataSource},
               {Public.Panels.PanelX},
               {Public.Panels.PanelY},
               {Public.Panels.PanelWidth},
               {Public.Panels.PanelHeight},
               {Public.Panels.PanelParentDashboardId}
           )
           VALUES (
               @PanelId,
               @PanelTitle,
               @PanelType,
               @PanelRawQuery,
               @PanelDataSource,
               @PanelX,
               @PanelY,
               @PanelWidth,
               @PanelHeight,
               @PanelParentDashboardId
           )
           """;

        var connection = await Factory.CreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot);
    }

    public async Task UpdateAsync(PanelEntity entity, CancellationToken token = default)
    {
        var panelSnapshot = entity.Save();

        var sql = $"""
           UPDATE {Public.PanelTable}
           SET 
               {Public.Panels.PanelTitle} = @PanelTitle,
               {Public.Panels.PanelType} = @PanelType,
               {Public.Panels.PanelRawQuery} = @PanelRawQuery,
               {Public.Panels.PanelDataSource} = @PanelDataSource,
               {Public.Panels.PanelX} = @PanelX,
               {Public.Panels.PanelY} = @PanelY,
               {Public.Panels.PanelWidth} = @PanelWidth,
               {Public.Panels.PanelHeight} = @PanelHeight,
               {Public.Panels.PanelParentDashboardId} = @PanelParentDashboardId
           WHERE {Public.Panels.PanelId} = @PanelId
           """;

        var connection = await Factory.CreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var sql = $"""
           DELETE FROM {Public.PanelTable}
           WHERE {Public.Panels.PanelId} = @PanelId
           """;

        var parameters = new { PanelId = id };

        var connection = await Factory.CreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters);
    }
}