namespace DataCat.Server.Postgres.Schemes;

public static class Public
{
    public static string PluginTable { get; } = PluginSnapshot.PluginTable.ToSnakeCase();
    public static string UserTable { get; } = UserSnapshot.UserTable.ToSnakeCase();
    public static string DataSourceTable { get; } = DataSourceSnapshot.DataSourceTable.ToSnakeCase();
    public static string DataSourceTypeTable { get; } = DataSourceTypeSnapshot.DataSourceTypeTable.ToSnakeCase();
    public static string DashboardTable { get; } = DashboardSnapshot.DashboardTable.ToSnakeCase();
    public static string PanelTable { get; } = PanelSnapshot.PanelTable.ToSnakeCase();
    public static string PanelTypeTable { get; } = PanelTypeSnapshot.PanelTypeTable.ToSnakeCase();
    public static string DashboardUserLinkTable { get; } = ManyToManyRelationShips.DashboardUserLinkTable.ToSnakeCase();

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
    
    public static class Users
    {
        public static string UserId { get; } = nameof(UserSnapshot.UserId).ToSnakeCase();
        public static string Name { get; } = nameof(UserSnapshot.Name).ToSnakeCase();
        public static string Email { get; } = nameof(UserSnapshot.Email).ToSnakeCase();
        public static string Role { get; } = nameof(UserSnapshot.Role).ToSnakeCase();
    }
    
    public static class DataSources
    {
        public static string DataSourceId { get; } = nameof(DataSourceSnapshot.DataSourceId).ToSnakeCase();
        public static string Name { get; } = nameof(DataSourceSnapshot.Name).ToSnakeCase();
        public static string DataSourceType { get; } = nameof(DataSourceSnapshot.DataSourceType).ToSnakeCase();
        public static string ConnectionString { get; } = nameof(DataSourceSnapshot.ConnectionString).ToSnakeCase();
    }

    public static class DataSourceTypes
    {
        public static string Id { get; } = nameof(DataSourceTypeSnapshot.Id).ToSnakeCase();
        public static string Source { get; } = nameof(DataSourceTypeSnapshot.Source).ToSnakeCase();
    }
    
    public static class Dashboards
    {
        public static string DashboardId { get; } = nameof(DashboardSnapshot.DashboardId).ToSnakeCase();
        public static string Name { get; } = nameof(DashboardSnapshot.Name).ToSnakeCase();
        public static string Description { get; } = nameof(DashboardSnapshot.Description).ToSnakeCase();
        public static string OwnerId { get; } = DashboardSnapshot.OwnerId.ToSnakeCase();
        public static string CreatedAt { get; } = nameof(DashboardSnapshot.CreatedAt).ToSnakeCase();
        public static string UpdatedAt { get; } = nameof(DashboardSnapshot.UpdatedAt).ToSnakeCase();
    }

    public static class Panels
    {
        public static string PanelId { get; } = nameof(PanelSnapshot.PanelId).ToSnakeCase();
        public static string Title { get; } = nameof(PanelSnapshot.Title).ToSnakeCase();
        public static string PanelType { get; } = nameof(PanelSnapshot.PanelType).ToSnakeCase();
        public static string RawQuery { get; } = nameof(PanelSnapshot.Query.RawQuery).ToSnakeCase();
        public static string DataSource { get; } = PanelSnapshot.DataSourceId.ToSnakeCase();
        public static string X { get; } = nameof(PanelSnapshot.X).ToSnakeCase();
        public static string Y { get; } = nameof(PanelSnapshot.Y).ToSnakeCase();
        public static string Width { get; } = nameof(PanelSnapshot.Width).ToSnakeCase();
        public static string Height { get; } = nameof(PanelSnapshot.Height).ToSnakeCase();
        public static string ParentDashboardId { get; } = nameof(PanelSnapshot.ParentDashboardId).ToSnakeCase();
    }

    public static class PanelTypes
    {
        public static string Id { get; } = nameof(PanelTypeSnapshot.Id).ToSnakeCase();
        public static string Type { get; } = nameof(PanelTypeSnapshot.Type).ToSnakeCase();
    }
}