namespace DataCat.Storage.Postgres.Repositories;

public sealed class NotificationChannelRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork,
    NotificationChannelManager NotificationChannelManager)
    : IRepository<NotificationChannelEntity, Guid>, INotificationChannelRepository
{
    public async Task<NotificationChannelEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_notification_channel_id = id.ToString() };

        const string sql = $"""
            SELECT
                {Public.NotificationChannels.Id}               {nameof(NotificationChannelSnapshot.Id)},
                {Public.NotificationChannels.DestinationId}    {nameof(NotificationChannelSnapshot.DestinationId)},
                {Public.NotificationChannels.Settings}         {nameof(NotificationChannelSnapshot.Settings)}
            FROM {Public.NotificationTable}
            WHERE {Public.NotificationChannels.Id} = @p_notification_channel_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QueryFirstOrDefaultAsync<NotificationChannelSnapshot>(sql, parameters, transaction: unitOfWork.Transaction);

        return result?.RestoreFromSnapshot(NotificationChannelManager);
    }

    public async Task AddAsync(NotificationChannelEntity entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();

        const string sql = $@"
            INSERT INTO {Public.NotificationTable} (
                {Public.NotificationChannels.Id},
                {Public.NotificationChannels.DestinationId},
                {Public.NotificationChannels.Settings}
            )
            VALUES (
                @{nameof(NotificationChannelSnapshot.Id)},
                @{nameof(NotificationChannelSnapshot.DestinationId)},
                @{nameof(NotificationChannelSnapshot.Settings)}
            )";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: unitOfWork.Transaction);
    }

    public async Task UpdateAsync(NotificationChannelEntity entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();

        const string sql = $"""
            UPDATE {Public.NotificationTable}
            SET 
                {Public.NotificationChannels.DestinationId} = @{nameof(NotificationChannelSnapshot.DestinationId)},
                {Public.NotificationChannels.Settings}      = @{nameof(NotificationChannelSnapshot.Settings)}
            WHERE {Public.NotificationChannels.Id} = @{nameof(NotificationChannelSnapshot.Id)}
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: unitOfWork.Transaction);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_notification_channel_id = id.ToString() };

        const string sql = $"""
            DELETE FROM {Public.NotificationTable}
            WHERE {Public.NotificationChannels.Id} = @p_notification_channel_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: unitOfWork.Transaction);
    }
}