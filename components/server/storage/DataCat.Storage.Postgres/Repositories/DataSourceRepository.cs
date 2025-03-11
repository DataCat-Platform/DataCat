namespace DataCat.Storage.Postgres.Repositories;

public sealed class DataSourceRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork)
    : IDefaultRepository<DataSourceEntity, Guid>
{
    public async Task<DataSourceEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var sql = $"SELECT * FROM {Public.DataSourceTable} WHERE {Public.DataSources.DataSourceId} = @DataSourceId";
        var parameters = new { DataSourceId = id.ToString() };

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QuerySingleOrDefaultAsync<DataSourceSnapshot>(sql, parameters, transaction: unitOfWork.Transaction);

        return result?.RestoreFromSnapshot();
    }

    public async IAsyncEnumerable<DataSourceEntity> SearchAsync(
        string? filter = null, 
        int page = 1, 
        int pageSize = 10, 
        [EnumeratorCancellation] CancellationToken token = default)
    {
        var offset = (page - 1) * pageSize;
        var sql = $"SELECT * FROM {Public.DataSourceTable} ";

        if (!string.IsNullOrEmpty(filter))
        {
            sql += $"WHERE {Public.DataSources.DataSourceName} LIKE @Filter ";
        }

        sql += "LIMIT @PageSize OFFSET @Offset";

        var parameters = new { Filter = $"{filter}%", PageSize = pageSize, Offset = offset };

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await using var reader = await connection.ExecuteReaderAsync(sql, parameters, transaction: unitOfWork.Transaction);

        while (await reader.ReadAsync(token))
        {
            var snapshot = reader.ReadDataSource();
            yield return snapshot.RestoreFromSnapshot();
        }
    }

    public async Task AddAsync(DataSourceEntity entity, CancellationToken token = default)
    {
        var panelSnapshot = entity.Save();

        var sql = $"""
           INSERT INTO {Public.DataSourceTable} (
               {Public.DataSources.DataSourceId},
               {Public.DataSources.DataSourceName},
               {Public.DataSources.DataSourceType},
               {Public.DataSources.DataSourceConnectionString}
           )
           VALUES (
               @DataSourceId,
               @DataSourceName,
               @DataSourceType,
               @DataSourceConnectionString
           )
           """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task UpdateAsync(DataSourceEntity entity, CancellationToken token = default)
    {
        var panelSnapshot = entity.Save();

        var sql = $"""
           UPDATE {Public.DataSourceTable}
           SET 
               {Public.DataSources.DataSourceName} = @DataSourceName,
               {Public.DataSources.DataSourceType} = @DataSourceType,
               {Public.DataSources.DataSourceConnectionString} = @DataSourceConnectionString
           WHERE {Public.DataSources.DataSourceId} = @DataSourceId
           """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var sql = $"""
               DELETE FROM {Public.DataSourceTable}
               WHERE {Public.DataSources.DataSourceId} = @DataSourceId
               """;

        var parameters = new { DataSourceId = id.ToString() };

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: unitOfWork.Transaction);
    }
}