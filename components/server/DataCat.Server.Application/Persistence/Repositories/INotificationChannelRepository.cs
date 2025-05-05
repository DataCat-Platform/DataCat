namespace DataCat.Server.Application.Persistence.Repositories;

public interface INotificationChannelRepository
{
    Task UpdateAsync(NotificationChannel entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
}