namespace DataCat.Storage.Postgres.Repositories;

public sealed class DataSourceRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork)
    : IRepository<DataSourceEntity, Guid>, IDataSourceRepository
{
    public async Task<DataSourceEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_data_source_id = id.ToString() };
        
        const string sql = $"""
            SELECT 
                {Public.DataSources.Id}                 {nameof(DataSourceSnapshot.Id)},
                {Public.DataSources.Name}               {nameof(DataSourceSnapshot.Name)},
                {Public.DataSources.TypeId}             {nameof(DataSourceSnapshot.TypeId)},
                {Public.DataSources.ConnectionString}   {nameof(DataSourceSnapshot.ConnectionString)}
            FROM {Public.DataSourceTable} 
            WHERE {Public.DataSources.Id} = @p_data_source_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QuerySingleOrDefaultAsync<DataSourceSnapshot>(sql, parameters, transaction: unitOfWork.Transaction);

        return result?.RestoreFromSnapshot();
    }
    
    public async Task AddAsync(DataSourceEntity entity, CancellationToken token = default)
    {
        var panelSnapshot = entity.Save();

        const string sql = $"""
            INSERT INTO {Public.DataSourceTable} (
                {Public.DataSources.Id},
                {Public.DataSources.Name},
                {Public.DataSources.TypeId},
                {Public.DataSources.ConnectionString}
            )
            VALUES (
                @{nameof(DataSourceSnapshot.Id)},
                @{nameof(DataSourceSnapshot.Name)},
                @{nameof(DataSourceSnapshot.TypeId)},
                @{nameof(DataSourceSnapshot.ConnectionString)}
            )
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task<Page<DataSourceEntity>> SearchAsync(
        string? filter = null, 
        int page = 1, 
        int pageSize = 10, 
        CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        
        var totalQueryArguments = new { p_name = $"{filter}%" };
        const string totalCountSql = DataSourceSql.Select.SearchDataSourcesTotalCount;
        var totalCount = await connection.QuerySingleAsync<int>(totalCountSql, totalQueryArguments);
        
        var offset = (page - 1) * pageSize;
        var parameters = new { p_name = $"{filter}%", limit = pageSize, offset = offset };
        const string sql = DataSourceSql.Select.SearchDataSources;
        var result = await connection.QueryAsync<DataSourceSnapshot>(sql, parameters, transaction: unitOfWork.Transaction);

        var items = result.Select(x => x.RestoreFromSnapshot());
        return new Page<DataSourceEntity>(items, totalCount, page, pageSize);
    }

    public async Task UpdateAsync(DataSourceEntity entity, CancellationToken token = default)
    {
        var panelSnapshot = entity.Save();

        const string sql = $"""
            UPDATE {Public.DataSourceTable}
            SET 
                {Public.DataSources.Name}             = @{nameof(DataSourceSnapshot.Name)},
                {Public.DataSources.TypeId}           = @{nameof(DataSourceSnapshot.TypeId)},
                {Public.DataSources.ConnectionString} = @{nameof(DataSourceSnapshot.ConnectionString)}
            WHERE {Public.DataSources.Id} = @{nameof(DataSourceSnapshot.Id)}
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_data_source_id = id.ToString() };

        const string sql = $"""
            DELETE FROM {Public.DataSourceTable}
            WHERE {Public.DataSources.Id} = @p_data_source_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: unitOfWork.Transaction);
    }
}