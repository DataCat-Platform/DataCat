namespace DataCat.Server.Api.Mappings;

public static class PluginMappingExtensions
{
    public static AddPluginCommand ToCommand(this AddPluginRequest request)
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
}