namespace DataCat.Server.Postgres.Snapshots;

public class PluginSnapshot
{
    public const string PluginTable = "plugin";
    
    public required string PluginId { get; init; }
    public required string Name { get; init; }
    public required string Version { get; init; }
    public string? Description { get; init; }
    public required string Author { get; init; }
    public bool IsEnabled { get; init; }
    public string? Settings { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public static class PluginEntitySnapshotMapper 
{
    public static PluginSnapshot Save(this PluginEntity pluginEntity)
    {
        return new PluginSnapshot
        {
            PluginId = pluginEntity.PluginId.ToString(),
            Name = pluginEntity.Name,
            Version = pluginEntity.Version,
            Description = pluginEntity.Description,
            Author = pluginEntity.Author,
            IsEnabled = pluginEntity.IsEnabled,
            Settings = pluginEntity.Settings,
            CreatedAt = pluginEntity.CreatedAt.ToUniversalTime(),
            UpdatedAt = pluginEntity.UpdatedAt.ToUniversalTime(),
        };
    }

    public static PluginEntity RestoreFromSnapshot(this PluginSnapshot snapshot)
    {
        var result = PluginEntity.Create(
            Guid.Parse(snapshot.PluginId),
            snapshot.Name,
            snapshot.Version,
            snapshot.Description,
            snapshot.Author,
            snapshot.IsEnabled,
            snapshot.Settings,
            snapshot.CreatedAt,
            snapshot.UpdatedAt);

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(PluginEntity));
    }
}