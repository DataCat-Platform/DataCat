namespace DataCat.Server.Application.Persistence.Repositories;

public interface INotificationChannelRepository
{
    Task UpdateAsync(NotificationChannel entity, CancellationToken token = default);

    Task DeleteAsync(int id, CancellationToken token = default);
}