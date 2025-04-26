namespace DataCat.Storage.Postgres.Repositories;

public sealed class DataSourceTypeRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork) : IDataSourceTypeRepository
{
    public async Task<DataSourceType?> GetByNameAsync(string name, CancellationToken token = default)
    {
        const string sql = $"""
            SELECT 
                {Public.DataSourceType.Id}    {nameof(DataSourceTypeSnapshot.Id)},
                {Public.DataSourceType.Name}  {nameof(DataSourceTypeSnapshot.Name)}
            FROM {Public.DataSourceTypeTable}
            WHERE {Public.DataSourceType.Name} ILIKE @{nameof(name)}
            LIMIT 1;
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QuerySingleOrDefaultAsync<DataSourceTypeSnapshot>(sql, new { name }, transaction: UnitOfWork.Transaction);
        return result?.RestoreFromSnapshot();
    }

    public async Task<int> AddAsync(DataSourceType dataSourceType, CancellationToken token = default)
    {
        var snapshot = dataSourceType.Save();

        const string sql = $"""
            INSERT INTO {Public.DataSourceTypeTable} (
                {Public.DataSourceType.Name}
            )
            VALUES (
                @{nameof(DataSourceType.Name)}
            )
            RETURNING {Public.DataSourceType.Id};
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var id = await connection.ExecuteScalarAsync<int>(sql, snapshot, transaction: UnitOfWork.Transaction);
        return id;
    }

    public async Task DeleteAsync(int id, CancellationToken token = default)
    {
        const string sql = $"""
            DELETE FROM {Public.DataSourceTypeTable}
            WHERE {Public.DataSourceType.Id} = @{nameof(id)};
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, new { id }, transaction: UnitOfWork.Transaction);
    }
}