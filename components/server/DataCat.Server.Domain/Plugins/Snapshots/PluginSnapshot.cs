namespace DataCat.Server.Domain.Plugins.Snapshots;

public class PluginSnapshot
{
    public required string PluginId { get; init; }
    public required string Name { get; init; }
    public required string Version { get; init; }
    public string? Description { get; init; }
    public required string Author { get; init; }
    public bool IsEnabled { get; init; }
    public string? Settings { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime? LastLoadedAt { get; init; }
}

public static class PluginSnapshotMapper 
{
    public static PluginSnapshot Save(this Plugin plugin)
    {
        return new PluginSnapshot
        {
            PluginId = plugin.PluginId.ToString(),
            Name = plugin.Name,
            Version = plugin.Version,
            Description = plugin.Description,
            Author = plugin.Author,
            IsEnabled = plugin.IsEnabled,
            Settings = plugin.Settings,
            CreatedAt = plugin.CreatedAt.ToUniversalTime(),
            UpdatedAt = plugin.UpdatedAt.ToUniversalTime(),
            LastLoadedAt = plugin.LastLoadedAt?.ToUniversalTime()
        };
    }

    public static Plugin RestoreFromSnapshot(this PluginSnapshot snapshot)
    {
        var result = Plugin.Create(
            Guid.Parse(snapshot.PluginId),
            snapshot.Name,
            snapshot.Version,
            snapshot.Description,
            snapshot.Author,
            snapshot.IsEnabled,
            snapshot.Settings,
            snapshot.CreatedAt,
            snapshot.UpdatedAt,
            snapshot.LastLoadedAt);

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(Plugin));
    }
}