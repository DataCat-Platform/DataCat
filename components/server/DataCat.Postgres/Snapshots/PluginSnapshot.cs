namespace DataCat.Server.Postgres.Snapshots;

public class PluginSnapshot
{
    public const string PluginTable = "plugins";
    
    public required string PluginId { get; init; }
    public required string PluginName { get; init; }
    public required string PluginVersion { get; init; }
    public string? PluginDescription { get; init; }
    public required string PluginAuthor { get; init; }
    public bool PluginIsEnabled { get; init; }
    public string? PluginSettings { get; init; }
    public DateTime PluginCreatedAt { get; init; }
    public DateTime PluginUpdatedAt { get; init; }
}

public static class PluginEntitySnapshotMapper 
{
    public static PluginSnapshot ReadPlugin(this DbDataReader reader)
    {
        return new PluginSnapshot
        {
            PluginId = reader.GetString(reader.GetOrdinal(Public.Plugins.PluginId)),
            PluginName = reader.GetString(reader.GetOrdinal(Public.Plugins.PluginName)),
            PluginVersion = reader.GetString(reader.GetOrdinal(Public.Plugins.PluginVersion)),
            PluginDescription = reader.IsDBNull(reader.GetOrdinal(Public.Plugins.PluginSettings)) 
                ? null 
                : reader.GetString(reader.GetOrdinal(Public.Plugins.PluginSettings)),
            PluginAuthor = reader.GetString(reader.GetOrdinal(Public.Plugins.PluginAuthor)),
            PluginIsEnabled = reader.GetBoolean(reader.GetOrdinal(Public.Plugins.PluginIsEnabled)),
            PluginSettings = reader.IsDBNull(reader.GetOrdinal(Public.Plugins.PluginSettings)) 
                ? null 
                : reader.GetString(reader.GetOrdinal(Public.Plugins.PluginSettings)),
            PluginCreatedAt = reader.GetDateTime(reader.GetOrdinal(Public.Plugins.PluginCreatedAt)),
            PluginUpdatedAt = reader.GetDateTime(reader.GetOrdinal(Public.Plugins.PluginUpdatedAt))
        };
    }
    
    public static PluginSnapshot Save(this PluginEntity pluginEntity)
    {
        return new PluginSnapshot
        {
            PluginId = pluginEntity.PluginId.ToString(),
            PluginName = pluginEntity.Name,
            PluginVersion = pluginEntity.Version,
            PluginDescription = pluginEntity.Description,
            PluginAuthor = pluginEntity.Author,
            PluginIsEnabled = pluginEntity.IsEnabled,
            PluginSettings = pluginEntity.Settings,
            PluginCreatedAt = pluginEntity.CreatedAt.ToUniversalTime(),
            PluginUpdatedAt = pluginEntity.UpdatedAt.ToUniversalTime(),
        };
    }

    public static PluginEntity RestoreFromSnapshot(this PluginSnapshot snapshot)
    {
        var result = PluginEntity.Create(
            Guid.Parse(snapshot.PluginId),
            snapshot.PluginName,
            snapshot.PluginVersion,
            snapshot.PluginDescription,
            snapshot.PluginAuthor,
            snapshot.PluginIsEnabled,
            snapshot.PluginSettings,
            snapshot.PluginCreatedAt,
            snapshot.PluginUpdatedAt);

        return result.IsSuccess ? result.Value : throw new DatabaseMappingException(typeof(PluginEntity));
    }
}