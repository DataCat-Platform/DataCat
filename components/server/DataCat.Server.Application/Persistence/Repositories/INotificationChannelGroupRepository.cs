namespace DataCat.Server.Application.Persistence.Repositories;

public interface INotificationChannelGroupRepository
{
    Task<Page<NotificationChannelGroup>> SearchAsync(
        SearchFilters filters,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);
    
    Task<NotificationChannelGroup?> GetByName(string name, CancellationToken cancellationToken = default);
}