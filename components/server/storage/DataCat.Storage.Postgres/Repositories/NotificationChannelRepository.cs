namespace DataCat.Storage.Postgres.Repositories;

public sealed class NotificationChannelRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork,
    NotificationChannelManager NotificationChannelManager)
    : IDefaultRepository<NotificationChannelEntity, Guid>
{
    public async Task<NotificationChannelEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { NotificationChannelId = id.ToString() };

        var sql = $@"
            SELECT * 
            FROM {Public.NotificationTable}
            WHERE {Public.NotificationChannels.NotificationChannelId} = @NotificationChannelId";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QueryFirstOrDefaultAsync<NotificationChannelSnapshot>(sql, parameters, transaction: unitOfWork.Transaction);

        return result?.RestoreFromSnapshot(NotificationChannelManager);
    }

    public async IAsyncEnumerable<NotificationChannelEntity> SearchAsync(
        string? filter = null, 
        int page = 1, 
        int pageSize = 10, 
        [EnumeratorCancellation] CancellationToken token = default)
    {
        var offset = (page - 1) * pageSize;
        var sql = $"SELECT * FROM {Public.NotificationTable} ";

        if (!string.IsNullOrEmpty(filter))
        {
            sql += $"WHERE {Public.NotificationChannels.NotificationSettings} LIKE @Filter ";
        }

        sql += "LIMIT @PageSize OFFSET @Offset";
        var parameters = new { Filter = $"%{filter}%", PageSize = pageSize, Offset = offset };

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await using var reader = await connection.ExecuteReaderAsync(sql, parameters, transaction: unitOfWork.Transaction);

        while (await reader.ReadAsync(token))
        {
            var snapshot = reader.ReadNotificationChannel();
            yield return snapshot.RestoreFromSnapshot(NotificationChannelManager);
        }
    }

    public async Task AddAsync(NotificationChannelEntity entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();

        var sql = $@"
            INSERT INTO {Public.NotificationTable} (
                {Public.NotificationChannels.NotificationChannelId},
                {Public.NotificationChannels.NotificationDestination},
                {Public.NotificationChannels.NotificationSettings}
            )
            VALUES (
                @NotificationChannelId,
                @NotificationDestination,
                @NotificationSettings
            )";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: unitOfWork.Transaction);
    }

    public async Task UpdateAsync(NotificationChannelEntity entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();

        var sql = $@"
            UPDATE {Public.NotificationTable}
            SET 
                {Public.NotificationChannels.NotificationDestination} = @NotificationDestination,
                {Public.NotificationChannels.NotificationSettings} = @NotificationSettings
            WHERE {Public.NotificationChannels.NotificationChannelId} = @NotificationChannelId";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: unitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { NotificationChannelId = id.ToString() };

        var sql = $@"
            DELETE FROM {Public.NotificationTable}
            WHERE {Public.NotificationChannels.NotificationChannelId} = @NotificationChannelId";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: unitOfWork.Transaction);
    }
}