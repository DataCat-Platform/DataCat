namespace DataCat.Server.Api.Mappings;

public static class PluginMappings
{
    public static AddPluginCommand ToAddCommand(this AddPluginRequest request)
    {
        return new AddPluginCommand
        {
            File = request.File,
            Name = request.Name,
            Version = request.Version,
            Description = request.Description,
            Author = request.Author,
            Settings = request.Settings
        };
    }
    
    public static UpdatePluginCommand ToUpdateCommand(this UpdatePluginRequest request, string pluginId)
    {
        return new UpdatePluginCommand
        {
            PluginId = pluginId,
            Description = request.Description,
            Settings = request.Settings
        };
    }

    public static FullPluginResponse ToResponse(this PluginEntity plugin)
    {
        return new FullPluginResponse()
        {
            PluginId = plugin.PluginId,
            Name = plugin.Name,
            Description = plugin.Description,
            Settings = plugin.Settings,
            Author = plugin.Author,
            Version = plugin.Version,
        };
    }
}
