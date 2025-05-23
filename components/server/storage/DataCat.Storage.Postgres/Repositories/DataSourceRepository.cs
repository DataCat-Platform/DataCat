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
                data_source.{Public.DataSources.Id}                   {nameof(DataSourceSnapshot.Id)},
                data_source.{Public.DataSources.Name}                 {nameof(DataSourceSnapshot.Name)},
                data_source.{Public.DataSources.TypeId}               {nameof(DataSourceSnapshot.TypeId)},
                data_source.{Public.DataSources.ConnectionSettings}   {nameof(DataSourceSnapshot.ConnectionSettings)},
                data_source.{Public.DataSources.Purpose}              {nameof(DataSourceSnapshot.Purpose)},
            
                data_source_type.{Public.DataSourceType.Id}            {nameof(DataSourceTypeSnapshot.Id)},
                data_source_type.{Public.DataSourceType.Name}          {nameof(DataSourceTypeSnapshot.Name)}
            
            FROM {Public.DataSourceTable} data_source 
            JOIN {Public.DataSourceTypeTable} data_source_type ON data_source.{Public.DataSources.TypeId} = data_source_type.{Public.DataSourceType.Id} 
            WHERE data_source.{Public.DataSources.Id} = @p_data_source_id
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
                {Public.DataSources.ConnectionSettings},
                {Public.DataSources.Purpose}
            )
            VALUES (
                @{nameof(DataSourceSnapshot.Id)},
                @{nameof(DataSourceSnapshot.Name)},
                @{nameof(DataSourceSnapshot.TypeId)},
                @{nameof(DataSourceSnapshot.ConnectionSettings)},
                @{nameof(DataSourceSnapshot.Purpose)}
            )
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, panelSnapshot, transaction: unitOfWork.Transaction);
    }

    public async Task<DataSource?> GetByNameAsync(string name, CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var parameters = new { p_name = name };
        const string sql = DataSourceSql.Select.GetByName;
        var result = await connection.QueryAsync<DataSourceSnapshot, DataSourceTypeSnapshot, DataSourceSnapshot>(sql,
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

    public async Task<IReadOnlyCollection<DataSource>> GetAllAsync(CancellationToken token = default)
    {
        var connection = await Factory.GetOrCreateConnectionAsync(token);

        const string sql = DataSourceSql.Select.GetAll;
        var result = await connection.QueryAsync<DataSourceSnapshot, DataSourceTypeSnapshot, DataSourceSnapshot>(sql,
            map: (dataSourceSnapshot, sourceTypeSnapshot) =>
            {
                dataSourceSnapshot.DataSourceType = sourceTypeSnapshot;
                return dataSourceSnapshot;
            },
            splitOn: $"{nameof(DataSourceTypeSnapshot.Id)}",
            transaction: unitOfWork.Transaction);

        return result.Select(x => x.RestoreFromSnapshot()).ToList();
    }

    public async Task<Page<DataSource>> SearchAsync(
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
            ["id"] = $"data_source.{Public.DataSources.Id}",
            ["name"] = $"data_source.{Public.DataSources.Name}",
            ["typeId"] = $"data_source.{Public.DataSources.TypeId}",
            ["purpose"] = $"data_source.{Public.DataSources.Purpose}"
        };

        var countSql = new StringBuilder();
        countSql.AppendLine(DataSourceSql.Select.SearchDataSourcesTotalCount);
        countSql.BuildQuery(parameters, filters, columnMappings);
        
        var countSqlString = countSql.ToString();
        
        var totalCount = await connection.QuerySingleAsync<int>(
            countSqlString,
            parameters,
            transaction: unitOfWork.Transaction);

        var dataSql = new StringBuilder();
        dataSql.AppendLine(DataSourceSql.Select.SearchDataSources);
        dataSql
            .BuildQuery(parameters, filters, columnMappings)
            .ApplyOrderBy(filters.Sort ?? new Sort(FieldName: "id"), columnMappings)
            .ApplyPagination();
        
        var dataSqlString = dataSql.ToString();

        var result = await connection.QueryAsync<DataSourceSnapshot, DataSourceTypeSnapshot, DataSourceSnapshot>(
            dataSqlString,
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
                {Public.DataSources.Name}               = @{nameof(DataSourceSnapshot.Name)},
                {Public.DataSources.TypeId}             = @{nameof(DataSourceSnapshot.TypeId)},
                {Public.DataSources.ConnectionSettings} = @{nameof(DataSourceSnapshot.ConnectionSettings)},
                {Public.DataSources.Purpose}            = @{nameof(DataSourceSnapshot.Purpose)}
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