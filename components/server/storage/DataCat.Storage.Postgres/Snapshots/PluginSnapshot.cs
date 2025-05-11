namespace DataCat.Storage.Postgres.Snapshots;

public sealed record PluginSnapshot
{
    public required string Id { get; init; }
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
    public static PluginSnapshot ReadPlugin(this DbDataReader reader)
    {
        return new PluginSnapshot
        {
            Id = reader.GetString(reader.GetOrdinal(Public.Plugins.Id)),
            Name = reader.GetString(reader.GetOrdinal(Public.Plugins.Name)),
            Version = reader.GetString(reader.GetOrdinal(Public.Plugins.Version)),
            Description = reader.IsDBNull(reader.GetOrdinal(Public.Plugins.Description)) 
                ? null 
                : reader.GetString(reader.GetOrdinal(Public.Plugins.Description)),
            Author = reader.GetString(reader.GetOrdinal(Public.Plugins.Author)),
            IsEnabled = reader.GetBoolean(reader.GetOrdinal(Public.Plugins.IsEnabled)),
            Settings = reader.IsDBNull(reader.GetOrdinal(Public.Plugins.Settings)) 
                ? null 
                : reader.GetString(reader.GetOrdinal(Public.Plugins.Settings)),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal(Public.Plugins.CreatedAt)),
            UpdatedAt = reader.GetDateTime(reader.GetOrdinal(Public.Plugins.UpdatedAt))
        };
    }
    
    public static PluginSnapshot Save(this Plugin plugin)
    {
        return new PluginSnapshot
        {
            Id = plugin.PluginId.ToString(),
            Name = plugin.Name,
            Version = plugin.Version,
            Description = plugin.Description,
            Author = plugin.Author,
            IsEnabled = plugin.IsEnabled,
            Settings = plugin.Settings,
            CreatedAt = plugin.CreatedAt.ToUniversalTime(),
            UpdatedAt = plugin.UpdatedAt.ToUniversalTime(),
        };
    }

    public static Plugin RestoreFromSnapshot(this PluginSnapshot snapshot)
    {
        var result = Plugin.Create(
            Guid.Parse(snapshot.Id),
            snapshot.Name,
            snapshot.Version,
            snapshot.Description,
            snapshot.Author,
            snapshot.IsEnabled,
            snapshot.Settings,
            snapshot.CreatedAt.ToUniversalTime(),
            snapshot.UpdatedAt.ToUniversalTime());

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(Plugin));
    }
}