namespace DataCat.Server.Application.Persistence.Repositories;

public interface IDashboardRepository
{
    Task AddUserToDashboard(User user, Dashboard dashboard, CancellationToken token = default);
    
    Task<Page<Dashboard>> SearchAsync(
        string? filter = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);
    
    Task UpdateAsync(Dashboard entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
}