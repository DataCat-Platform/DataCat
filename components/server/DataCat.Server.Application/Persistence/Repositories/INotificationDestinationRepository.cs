namespace DataCat.Server.Application.Persistence.Repositories;

public interface INotificationDestinationRepository
{
    Task<NotificationDestination?> GetByNameAsync(string name, CancellationToken token = default);
    
    Task<int> AddAsync(NotificationDestination destination, CancellationToken token = default);
    
    Task DeleteAsync(int id, CancellationToken token = default);
}