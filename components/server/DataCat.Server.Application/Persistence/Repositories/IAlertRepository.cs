namespace DataCat.Server.Application.Persistence.Repositories;

public interface IAlertRepository
{
    Task<List<AlertCounterResponse>> GetAlertCountersAsync(CancellationToken token = default);
    
    Task<Page<Alert>> SearchAsync(
        SearchFilters filters,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);

    Task UpdateAsync(Alert entity, CancellationToken token = default);
    
    Task DeleteAsync(Guid id, CancellationToken token = default);
}