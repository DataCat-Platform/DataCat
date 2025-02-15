namespace DataCat.Server.Domain.Plugins.Repos;

public interface ITogglePluginStatusRepository
{
    Task<bool> ToggleStatusAsync(Guid id, bool isEnabled, CancellationToken token = default);
}