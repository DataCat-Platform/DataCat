namespace DataCat.Server.Postgres.Schemes;

public static class Public
{
    public static string PluginTable { get; } = PluginSnapshot.PluginTable.ToSnakeCase();

    public static class Plugins
    {
        public static string PluginId { get; } = nameof(PluginSnapshot.PluginId).ToSnakeCase();
        public static string Name { get; } = nameof(PluginSnapshot.Name).ToSnakeCase();
        public static string Version { get; } = nameof(PluginSnapshot.Version).ToSnakeCase();
        public static string Description { get; } = nameof(PluginSnapshot.Description).ToSnakeCase();
        public static string Author { get; } = nameof(PluginSnapshot.Author).ToSnakeCase();
        public static string IsEnabled { get; } = nameof(PluginSnapshot.IsEnabled).ToSnakeCase();
        public static string Settings { get; } = nameof(PluginSnapshot.Settings).ToSnakeCase();
        public static string CreatedAt { get; } = nameof(PluginSnapshot.CreatedAt).ToSnakeCase();
        public static string UpdatedAt { get; } = nameof(PluginSnapshot.UpdatedAt).ToSnakeCase();
    } 
}