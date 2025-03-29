namespace DataCat.Storage.Postgres.Snapshots;

public sealed class PluginSnapshot
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
    
    public static PluginSnapshot Save(this PluginEntity pluginEntity)
    {
        return new PluginSnapshot
        {
            Id = pluginEntity.PluginId.ToString(),
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
            Guid.Parse(snapshot.Id),
            snapshot.Name,
            snapshot.Version,
            snapshot.Description,
            snapshot.Author,
            snapshot.IsEnabled,
            snapshot.Settings,
            snapshot.CreatedAt.ToUniversalTime(),
            snapshot.UpdatedAt.ToUniversalTime());

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(PluginEntity));
    }
}