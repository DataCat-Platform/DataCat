namespace DataCat.Server.DI;

public static class PluginLoader
{
    public static void LoadDatabasePlugin(IServiceCollection services, string provider, IConfiguration configuration)
    {
        var pluginDirectory = AppContext.BaseDirectory;

        var assemblyFile = provider switch
        {
            "postgres" => Path.Combine(pluginDirectory, "DataCat.Storage.Postgres.dll"),
            _ => throw new Exception("Unsupported database provider")
        };

        if (!File.Exists(assemblyFile))
            throw new Exception($"Plugin assembly not found: {assemblyFile}");

        var assembly = Assembly.LoadFrom(assemblyFile);

        var pluginType = assembly.GetTypes()
            .FirstOrDefault(t => typeof(IDatabasePlugin).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        if (pluginType == null)
            throw new Exception($"No implementation of {nameof(IDatabasePlugin)} found in {assemblyFile}");

        // Create database plugin instance to register database
        if (Activator.CreateInstance(pluginType) is IDatabasePlugin plugin)
        {
            plugin.RegisterRepositories(services, configuration);
        }
        else
        {
            throw new Exception($"Failed to create an instance of {pluginType.FullName}");
        }
    }
}