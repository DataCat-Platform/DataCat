namespace DataCat.Storage.Postgres.Repositories;

public sealed class NotificationChannelRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork,
    NotificationChannelManager NotificationChannelManager)
    : IRepository<NotificationChannel, Guid>, INotificationChannelRepository
{
    public async Task<NotificationChannel?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var parameters = new { p_notification_channel_id = id.ToString() };

        const string sql = $"""
            SELECT
                notification.{Public.NotificationChannels.Id}               {nameof(NotificationChannelSnapshot.Id)},
                notification.{Public.NotificationChannels.DestinationId}    {nameof(NotificationChannelSnapshot.DestinationId)},
                notification.{Public.NotificationChannels.Settings}         {nameof(NotificationChannelSnapshot.Settings)},
                
                notification_destination.{Public.NotificationChannels.Id}               {nameof(NotificationChannelSnapshot.Id)},
                notification_destination.{Public.NotificationChannels.Id}               {nameof(NotificationChannelSnapshot.Id)}
            
            FROM 
                {Public.NotificationChannelTable} notification
            JOIN 
                {Public.NotificationDestinationTable} notification_destination 
                    ON notification.{Public.NotificationChannels.DestinationId} = notification_destination.{Public.NotificationChannels.Id} 
            WHERE {Public.NotificationChannels.Id} = @p_notification_channel_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        var result = await connection.QueryAsync<NotificationChannelSnapshot, NotificationDestinationSnapshot, NotificationChannelSnapshot>(sql,
            map: (notification, destination) =>
            {
                notification.Destination = destination;
                return notification;
            }, 
            param: parameters, 
            transaction: unitOfWork.Transaction);

        return result.FirstOrDefault()?.RestoreFromSnapshot(NotificationChannelManager);
    }

    public async Task AddAsync(NotificationChannel entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();

        const string sql = $@"
            INSERT INTO {Public.NotificationChannelTable} (
                {Public.NotificationChannels.DestinationId},
                {Public.NotificationChannels.Settings}
            )
            VALUES (
                @{nameof(NotificationChannelSnapshot.DestinationId)},
                @{nameof(NotificationChannelSnapshot.Settings)}
            )
            RETURNING {Public.NotificationChannels.Id};
        ";

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: unitOfWork.Transaction);
    }

    public async Task UpdateAsync(NotificationChannel entity, CancellationToken token = default)
    {
        var snapshot = entity.Save();

        const string sql = $"""
            UPDATE {Public.NotificationChannelTable}
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
            DELETE FROM {Public.NotificationChannelTable}
            WHERE {Public.NotificationChannels.Id} = @p_notification_channel_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: unitOfWork.Transaction);
    }
}