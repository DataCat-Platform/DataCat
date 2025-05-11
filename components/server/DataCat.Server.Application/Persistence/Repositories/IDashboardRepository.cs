namespace DataCat.Server.Application.Persistence.Repositories;

public interface IDashboardRepository
{
    Task<Page<Dashboard>> SearchAsync(
        SearchFilters filters,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);
    
    Task<IReadOnlyCollection<DashboardResponse>> GetDashboardsByNamespaceId(Guid id, CancellationToken token = default);
    
    Task UpdateAsync(Dashboard entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
}