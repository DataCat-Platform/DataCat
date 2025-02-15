namespace DataCat.Server.Postgres.Schemes;

public static class Public
{
    public static string PluginTable { get; } = PluginSnapshot.PluginTable.ToSnakeCase();
    public static string UserTable { get; } = UserSnapshot.UserTable.ToSnakeCase();
    public static string DataSourceTable { get; } = DataSourceSnapshot.DataSourceTable.ToSnakeCase();
    public static string DashboardTable { get; } = DashboardSnapshot.DashboardTable.ToSnakeCase();
    public static string PanelTable { get; } = PanelSnapshot.PanelTable.ToSnakeCase();
    public static string DashboardUserLinkTable { get; } = ManyToManyRelationShips.DashboardUserLinkTable.ToSnakeCase();

    public static class Plugins
    {
        public static string PluginId { get; } = nameof(PluginSnapshot.PluginId).ToSnakeCase();
        public static string PluginName { get; } = nameof(PluginSnapshot.PluginName).ToSnakeCase();
        public static string PluginVersion { get; } = nameof(PluginSnapshot.PluginVersion).ToSnakeCase();
        public static string PluginDescription { get; } = nameof(PluginSnapshot.PluginDescription).ToSnakeCase();
        public static string PluginAuthor { get; } = nameof(PluginSnapshot.PluginAuthor).ToSnakeCase();
        public static string PluginIsEnabled { get; } = nameof(PluginSnapshot.PluginIsEnabled).ToSnakeCase();
        public static string PluginSettings { get; } = nameof(PluginSnapshot.PluginSettings).ToSnakeCase();
        public static string PluginCreatedAt { get; } = nameof(PluginSnapshot.PluginCreatedAt).ToSnakeCase();
        public static string PluginUpdatedAt { get; } = nameof(PluginSnapshot.PluginUpdatedAt).ToSnakeCase();
    }
    
    public static class Users
    {
        public static string UserId { get; } = nameof(UserSnapshot.UserId).ToSnakeCase();
        public static string UserName { get; } = nameof(UserSnapshot.UserName).ToSnakeCase();
        public static string UserEmail { get; } = nameof(UserSnapshot.UserEmail).ToSnakeCase();
        public static string UserRole { get; } = nameof(UserSnapshot.UserRole).ToSnakeCase();
    }
    
    public static class DataSources
    {
        public static string DataSourceId { get; } = nameof(DataSourceSnapshot.DataSourceId).ToSnakeCase();
        public static string DataSourceName { get; } = nameof(DataSourceSnapshot.DataSourceName).ToSnakeCase();
        public static string DataSourceType { get; } = nameof(DataSourceSnapshot.DataSourceType).ToSnakeCase();
        public static string DataSourceConnectionString { get; } = nameof(DataSourceSnapshot.DataSourceConnectionString).ToSnakeCase();
    }
    
    public static class Dashboards
    {
        public static string DashboardId { get; } = nameof(DashboardSnapshot.DashboardId).ToSnakeCase();
        public static string DashboardName { get; } = nameof(DashboardSnapshot.DashboardName).ToSnakeCase();
        public static string DashboardDescription { get; } = nameof(DashboardSnapshot.DashboardDescription).ToSnakeCase();
        public static string DashboardOwnerId { get; } = DashboardSnapshot.DashboardOwnerId.ToSnakeCase();
        public static string DashboardCreatedAt { get; } = nameof(DashboardSnapshot.DashboardCreatedAt).ToSnakeCase();
        public static string DashboardUpdatedAt { get; } = nameof(DashboardSnapshot.DashboardUpdatedAt).ToSnakeCase();
    }

    public static class Panels
    {
        public static string PanelId { get; } = nameof(PanelSnapshot.PanelId).ToSnakeCase();
        public static string PanelTitle { get; } = nameof(PanelSnapshot.PanelTitle).ToSnakeCase();
        public static string PanelType { get; } = nameof(PanelSnapshot.PanelType).ToSnakeCase();
        public static string PanelRawQuery { get; } = nameof(PanelSnapshot.PanelRawQuery).ToSnakeCase();
        public static string PanelDataSourceId { get; } = PanelSnapshot.Panel_DataSourceId.ToSnakeCase();
        public static string PanelX { get; } = nameof(PanelSnapshot.PanelX).ToSnakeCase();
        public static string PanelY { get; } = nameof(PanelSnapshot.PanelY).ToSnakeCase();
        public static string PanelWidth { get; } = nameof(PanelSnapshot.PanelWidth).ToSnakeCase();
        public static string PanelHeight { get; } = nameof(PanelSnapshot.PanelHeight).ToSnakeCase();
        public static string PanelParentDashboardId { get; } = nameof(PanelSnapshot.PanelParentDashboardId).ToSnakeCase();
    }
}