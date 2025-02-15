namespace DataCat.Server.Infrastructure.Repositories;

public class DiskPluginStorage(PluginStoreOptions pluginOptions) : IPluginStorage
{
    public async Task<string> SavePlugin(string fileName, Stream file, CancellationToken cancellationToken = default)
    {
        var directoryPath = Path.Combine(AppContext.BaseDirectory, pluginOptions.PluginPath);
        
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        var filePath = Path.Combine(directoryPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return filePath;
    }

    public PluginInfo FindPluginById(string id, CancellationToken cancellationToken = default)
    {
        var directoryPath = Path.Combine(AppContext.BaseDirectory, pluginOptions.PluginPath);
        var filePath = Path.Combine(directoryPath, id);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Plugin not found", id);
        }
        
        return new PluginInfo { FilePath = filePath, FileName = Path.GetFileName(filePath) };
    }

    public List<PluginInfo> GetAllPlugins(CancellationToken cancellationToken = default)
    {
        var directoryPath = Path.Combine(AppContext.BaseDirectory, pluginOptions.PluginPath);
        
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException("Plugin not found");
        }
        
        var files = Directory.GetFiles(directoryPath);
        return files.Select(file => new PluginInfo { FilePath = file, FileName = Path.GetFileName(file) }).ToList();
    }

    public void DeletePlugin(string id, CancellationToken cancellationToken = default)
    {
        var directoryPath = Path.Combine(AppContext.BaseDirectory, pluginOptions.PluginPath);
        var filePath = Path.Combine(directoryPath, id);
        
        File.Delete(filePath);
    }
}