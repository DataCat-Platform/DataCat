namespace DataCat.Server.Application.Persistence.Repositories;

public interface IPluginRepository
{
    Task<Page<Plugin>> SearchAsync(
        SearchFilters filters,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);
    
    Task UpdateAsync(Plugin entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
    
    Task<bool> ToggleStatusAsync(Guid id, bool isEnabled, CancellationToken token = default);
}