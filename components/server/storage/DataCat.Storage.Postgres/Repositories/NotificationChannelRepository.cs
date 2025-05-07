namespace DataCat.Storage.Postgres.Repositories;

public sealed class NotificationChannelRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork,
    NotificationChannelManager NotificationChannelManager)
    : IRepository<NotificationChannel, int>, INotificationChannelRepository
{
    public async Task<NotificationChannel?> GetByIdAsync(int id, CancellationToken token = default)
    {
        var parameters = new { p_notification_channel_id = id };

        const string sql = $"""
            SELECT
                notification.{Public.NotificationChannels.Id}                             {nameof(NotificationChannelSnapshot.Id)},
                notification.{Public.NotificationChannels.DestinationId}                  {nameof(NotificationChannelSnapshot.DestinationId)},
                notification.{Public.NotificationChannels.Settings}                       {nameof(NotificationChannelSnapshot.Settings)},
                notification.{Public.NotificationChannels.NotificationChannelGroupId}     {nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
                
                notification_destination.{Public.NotificationDestination.Id}                 {nameof(NotificationDestinationSnapshot.Id)},
                notification_destination.{Public.NotificationDestination.Name}               {nameof(NotificationDestinationSnapshot.Name)}
            
            FROM 
                {Public.NotificationChannelTable} notification
            JOIN 
                {Public.NotificationDestinationTable} notification_destination 
                    ON notification.{Public.NotificationChannels.DestinationId} = notification_destination.{Public.NotificationChannels.Id} 
            WHERE notification.{Public.NotificationChannels.Id} = @p_notification_channel_id
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
                {Public.NotificationChannels.NotificationChannelGroupId},
                {Public.NotificationChannels.DestinationId},
                {Public.NotificationChannels.Settings}
            )
            VALUES (
                @{nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
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
                {Public.NotificationChannels.NotificationChannelGroupId} = @{nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
                {Public.NotificationChannels.DestinationId}              = @{nameof(NotificationChannelSnapshot.DestinationId)},
                {Public.NotificationChannels.Settings}                   = @{nameof(NotificationChannelSnapshot.Settings)}
            WHERE {Public.NotificationChannels.Id} = @{nameof(NotificationChannelSnapshot.Id)}
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, snapshot, transaction: unitOfWork.Transaction);
    }

    public async Task DeleteAsync(int id, CancellationToken token = default)
    {
        var parameters = new { p_notification_channel_id = id };

        const string sql = $"""
            DELETE FROM {Public.NotificationChannelTable}
            WHERE {Public.NotificationChannels.Id} = @p_notification_channel_id
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);
        await connection.ExecuteAsync(sql, parameters, transaction: unitOfWork.Transaction);
    }
}