namespace DataCat.Server.Application.Queries.Plugins.Search;

public sealed record SearchPluginsResponse
{
    public required Guid PluginId { get; init; }
    public required string Name { get; init; }
    public required string? Version { get; init; }
    public string? Description { get; init; }
    public required string Author { get; init; }
    public string? Settings { get; init; }
    
    public static SearchPluginsResponse ToResponse(Plugin plugin)
    {
        return new SearchPluginsResponse
        {
            PluginId = plugin.PluginId,
            Name = plugin.Name,
            Description = plugin.Description,
            Settings = plugin.Settings,
            Author = plugin.Author,
            Version = plugin.Version
        };
    }
}
