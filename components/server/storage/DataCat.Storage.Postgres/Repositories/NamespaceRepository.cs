namespace DataCat.Storage.Postgres.Repositories;

public sealed class NamespaceRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork)
    : IRepository<Namespace, Guid>, INamespaceRepository
{
    public async Task<Namespace?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_id = id.ToString() };
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        
        const string sql = $"""
            SELECT 
                {Public.Namespaces.Id}       {nameof(NamespaceSnapshot.Id)}, 
                {Public.Namespaces.Name}     {nameof(NamespaceSnapshot.Name)}
            FROM {Public.NamespaceTable}
            WHERE {Public.Namespaces.Id} = @p_id
        """;

        var result = await connection.QuerySingleOrDefaultAsync<NamespaceSnapshot>(
            sql,
            param: parameters,
            transaction: unitOfWork.Transaction
        );

        return result?.RestoreFromSnapshot();
    }
    
    public async ValueTask<Namespace?> GetByNameAsync(string name, CancellationToken token)
    {
        var parameters = new { p_name = name };
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        
        const string sql = $"""
            SELECT 
                {Public.Namespaces.Id}       {nameof(NamespaceSnapshot.Id)}, 
                {Public.Namespaces.Name}     {nameof(NamespaceSnapshot.Name)}
            FROM {Public.NamespaceTable}
            WHERE {Public.Namespaces.Name} = @p_name
        """;

        var result = await connection.QuerySingleOrDefaultAsync<NamespaceSnapshot>(
            sql,
            param: parameters,
            transaction: unitOfWork.Transaction
        );

        return result?.RestoreFromSnapshot();
    }

    public async ValueTask<Namespace> GetDefaultNamespaceAsync(CancellationToken token)
    {
        return (await GetByNameAsync(ApplicationConstants.DefaultNamespace, token))!;
    }

    public async Task AddAsync(Namespace entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();

        const string sql = $"""
            INSERT INTO {Public.NamespaceTable} (
                {Public.Namespaces.Id},
                {Public.Namespaces.Name}
            )
            VALUES (
                @{nameof(NamespaceSnapshot.Id)},
                @{nameof(NamespaceSnapshot.Name)}
            );
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: unitOfWork.Transaction);
    }
}