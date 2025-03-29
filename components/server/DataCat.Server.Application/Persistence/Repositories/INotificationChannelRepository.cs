namespace DataCat.Server.Application.Persistence.Repositories;

public interface INotificationChannelRepository
{
    Task UpdateAsync(NotificationChannelEntity entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
}