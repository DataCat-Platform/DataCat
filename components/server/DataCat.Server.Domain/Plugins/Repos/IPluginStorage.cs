namespace DataCat.Server.Domain.Plugins.Repos;

public interface IPluginStorage
{
    Task<string> SavePlugin(string fileName, Stream file, CancellationToken cancellationToken = default);
    
    PluginInfo FindPluginById(string id, CancellationToken cancellationToken = default);
    
    List<PluginInfo> GetAllPlugins(CancellationToken cancellationToken = default);
    
    void DeletePlugin(string id, CancellationToken cancellationToken = default);
}