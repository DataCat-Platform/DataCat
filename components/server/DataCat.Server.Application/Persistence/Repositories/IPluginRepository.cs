namespace DataCat.Server.Application.Persistence.Repositories;

public interface IPluginRepository
{
    Task<Page<PluginEntity>> SearchAsync(
        string? filter = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);
    
    Task UpdateAsync(PluginEntity entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
    
    Task<bool> ToggleStatusAsync(Guid id, bool isEnabled, CancellationToken token = default);
}