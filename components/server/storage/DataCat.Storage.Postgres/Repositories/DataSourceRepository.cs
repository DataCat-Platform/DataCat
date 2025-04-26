namespace DataCat.Storage.Postgres.Repositories;

public sealed class DataSourceRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork)
    : IRepository<DataSource, Guid>, IDataSourceRepository
{
    public async Task<DataSource?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_data_source_id = id.ToString() };
        
        const string sql = $"""
            SELECT 
                data_source.{Public.DataSources.Id}                 {nameof(DataSourceSnapshot.Id)},
                data_source.{Public.DataSources.Name}               {nameof(DataSourceSnapshot.Name)},
                data_source.{Public.DataSources.TypeId}             {nameof(DataSourceSnapshot.TypeId)},
                data_source.{Public.DataSources.ConnectionString}   {nameof(DataSourceSnapshot.ConnectionString)},
            
                data_source_type.{Public.DataSourceType.Id}            {nameof(DataSourceTypeSnapshot.Id)},
                data_source_type.{Public.DataSourceType.Name}          {nameof(DataSourceTypeSnapshot.Name)}
            
            FROM {Public.DataSourceTable} data_source 
            JOIN {Public.DataSourceTypeTable} data_source_type ON data_source.{Public.DataSources.TypeId} = data_source_type.{Public.DataSourceType.Id} 
            WHERE {Public.DataSources.Id} = @p_data_source_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QueryAsync<DataSourceSnapshot, DataSourceTypeSnapshot, DataSourceSnapshot>(
            sql,
            map: (dataSourceSnapshot, sourceTypeSnapshot) =>
            {
                dataSourceSnapshot.DataSourceType = sourceTypeSnapshot;
                return dataSourceSnapshot;
            },
            splitOn: $"{nameof(DataSourceTypeSnapshot.Id)}",
            param: parameters, 
            transaction: unitOfWork.Transaction);

        return result.FirstOrDefault()?.RestoreFromSnapshot();
    }
    
    public async Task AddAsync(DataSource entity, CancellationToken token = default)
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

    public async Task<Page<DataSource>> SearchAsync(
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
        var result = await connection.QueryAsync<DataSourceSnapshot, DataSourceTypeSnapshot, DataSourceSnapshot>(sql,
            map: (dataSourceSnapshot, sourceTypeSnapshot) =>
            {
                dataSourceSnapshot.DataSourceType = sourceTypeSnapshot;
                return dataSourceSnapshot;
            },
            splitOn: $"{nameof(DataSourceTypeSnapshot.Id)}",
            param: parameters, 
            transaction: unitOfWork.Transaction);

        var items = result.Select(x => x.RestoreFromSnapshot());
        return new Page<DataSource>(items, totalCount, page, pageSize);
    }

    public async Task UpdateAsync(DataSource entity, CancellationToken token = default)
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