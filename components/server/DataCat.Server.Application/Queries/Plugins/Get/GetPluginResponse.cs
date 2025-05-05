namespace DataCat.Server.Application.Queries.Plugins.Get;

public sealed record GetPluginResponse
{
    public required Guid PluginId { get; init; }
    public required string Name { get; init; }
    public required string? Version { get; init; }
    public string? Description { get; init; }
    public required string Author { get; init; }
    public string? Settings { get; init; }
}

public static class GetPluginResponseExtensions
{
    public static GetPluginResponse ToResponse(this Plugin plugin)
    {
        return new GetPluginResponse
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