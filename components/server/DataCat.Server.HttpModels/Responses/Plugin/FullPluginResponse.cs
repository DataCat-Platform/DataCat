namespace DataCat.Server.HttpModels.Responses.Plugin;

public class FullPluginResponse
{
    public required Guid PluginId { get; init; }
    
    public required string Name { get; init; }

    public required string? Version { get; init; }

    public string? Description { get; init; }

    public required string Author { get; init; }

    public string? Settings { get; init; }
}