namespace DataCat.Storage.Postgres.Repositories;

public sealed class NotificationDestinationRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork UnitOfWork) : INotificationDestinationRepository
{
    public async Task<NotificationDestination?> GetByNameAsync(string name, CancellationToken token = default)
    {
        const string sql = $"""
            SELECT 
                {Public.NotificationDestination.Id}       {nameof(NotificationDestinationSnapshot.Id)}, 
                {Public.NotificationDestination.Name}     {nameof(NotificationDestinationSnapshot.Name)}
            FROM {Public.NotificationDestinationTable}
            WHERE {Public.NotificationDestination.Name} ILIKE @{nameof(name)}
            LIMIT 1;
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QuerySingleOrDefaultAsync<NotificationDestinationSnapshot>(sql, new { name }, transaction: UnitOfWork.Transaction);
        return result?.RestoreFromSnapshot();
    }

    public async Task<int> AddAsync(NotificationDestination destination, CancellationToken token = default)
    {
        var snapshot = destination.Save();

        const string sql = $"""
            INSERT INTO {Public.NotificationDestinationTable} (
                {Public.NotificationDestination.Name}
            )
            VALUES (
                @{nameof(NotificationDestination.Name)}
            )
            RETURNING {Public.NotificationDestination.Id};
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var id = await connection.ExecuteScalarAsync<int>(sql, snapshot, transaction: UnitOfWork.Transaction);
        return id;
    }

    public async Task DeleteAsync(int id, CancellationToken token = default)
    {
        const string sql = $"""
            DELETE FROM {Public.NotificationDestinationTable}
            WHERE {Public.NotificationDestination.Id} = @{nameof(id)};
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, new { id }, transaction: UnitOfWork.Transaction);
    }
}