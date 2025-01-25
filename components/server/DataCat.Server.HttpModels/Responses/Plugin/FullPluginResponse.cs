namespace DataCat.Server.HttpModels.Responses.Plugin;

public class FullPluginResponse
{
    public string? PluginId { get; set; }
    
    public string? Name { get; set; }

    public string? Version { get; set; }

    public string? Description { get; set; }

    public string? Author { get; set; }

    public string? Settings { get; set; }
}