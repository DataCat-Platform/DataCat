namespace DataCat.Storage.Postgres.Schemes;

public static class Public
{
    public const string PluginTable = "plugins";
    public const string UserTable = "datacat_users";
    public const string DataSourceTable = "data_sources";
    public const string DashboardTable = "dashboards";
    public const string PanelTable = "panels";
    public const string AlertTable = "alerts";
    public const string NotificationTable = "notification_channel";
    public const string DashboardUserLinkTable = "dashboard_user_link";

    public static class Plugins
    {
        public const string Id = "pk_plugin_id";
        public const string Name = "name";
        public const string Version = "version";
        public const string Description = "description";
        public const string Author = "author";
        public const string IsEnabled = "is_enabled";
        public const string Settings = "settings";
        public const string CreatedAt = "created_at_utc";
        public const string UpdatedAt = "updated_at_utc";
    }
    
    public static class Users
    {
        public const string Id = "pk_user_id";
    }
    
    public static class DataSources
    {
        public const string Id = "pk_datasource_id";
        public const string Name = "name";
        public const string TypeId = "type_id";
        public const string ConnectionString = "connection_string";
    }
    
    public static class Dashboards
    {
        public const string Id = "pk_dashboard_id";
        public const string Name = "name";
        public const string Description = "description";
        public const string OwnerId = "owner_id";
        public const string CreatedAt = "created_at_utc";
        public const string UpdatedAt = "updated_at_utc";
    }

    public static class Panels
    {
        public const string Id = "pk_panel_id";
        public const string Title = "title";
        public const string TypeId = "type_id";
        public const string RawQuery = "raw_query";
        public const string DataSourceId = "data_source_id";
        public const string X = "x";
        public const string Y = "y";
        public const string Width = "width";
        public const string Height = "height";
        public const string DashboardId = "dashboard_id";
    }

    public static class Alerts
    {
        public const string Id = "pk_alert_id";
        public const string Description = "description";
        public const string Status = "status";
        public const string RawQuery = "raw_query";
        public const string DataSourceId = "data_source_id";
        public const string NotificationChannelId = "notification_channel_id";
        public const string PreviousExecution = "previous_execution";
        public const string NextExecution = "next_execution";
        public const string WaitTimeBeforeAlertingInTicks = "wait_time_before_alerting_in_ticks";
        public const string RepeatIntervalInTicks = "repeat_interval_in_ticks";
    }

    public static class NotificationChannels
    {
        public const string Id = "pk_notification_id";
        public const string DestinationId = "destination_id";
        public const string Settings = "settings";
    }
}