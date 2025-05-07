namespace DataCat.Storage.Postgres.Schemes;

public static class Public
{
    public const string PluginTable = "plugins";
    
    public const string UserTable = "datacat_users";
    public const string RoleTable = "datacat_roles";
    public const string PermissionsTable = "datacat_permissions";
    public const string NamespaceTable = "datacat_namespaces";
    public const string ExternalRoleMappingTable = "external_role_mappings";
    public const string ExternalPermissionMappingTable = "external_permission_mappings";

    public const string AlertTable = "alerts";
    public const string DashboardTable = "dashboards";
    public const string DataSourceTable = "data_sources";
    public const string DataSourceTypeTable = "data_source_type";
    public const string NotificationDestinationTable = "notification_destination";
    public const string NotificationChannelTable = "notification_channel";
    public const string NotificationChannelGroupTable = "notification_channel_group";
    public const string PanelTable = "panels";
    public const string VariableTable = "variables";
    
    public const string DashboardUserLinkTable = "dashboard_user_link";
    public const string UserRoleLinkTable = "user_role_link";
    public const string UserPermissionLinkTable = "user_permission_link";

    public static class Plugins
    {
        public const string Id = "id";
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
        public const string Id = "id";
        public const string IdentityId = "identity_id";
        public const string Email = "email";
        public const string Name = "name";
        public const string CreatedAt = "created_at_utc";
        public const string UpdatedAt = "updated_at_utc";
    }

    public static class Roles
    {
        public const string Id = "id";
        public const string Name = "name";
    }
    
    public static class UsersRolesLink
    {
        public const string UserId = "user_id";
        public const string RoleId = "role_id";
        public const string NamespaceId = "namespace_id";
        public const string IsManual = "is_manual";
    }

    public static class Permissions
    {
        public const string Id = "id";
        public const string Name = "name";
    }

    public static class UsersPermissionsLink
    {
        public const string UserId = Users.Id;
        public const string PermissionId = "permission_id";
        public const string NamespaceId = "namespace_id";
        public const string IsManual = "is_manual";
    }

    public static class Namespaces
    {
        public const string Id = "id";
        public const string Name = "name";
    }

    public static class ExternalRoleMappings
    {
        public const string ExternalRole = "external_role";
        public const string InternalRoleId = "role_id";
        public const string NamespaceId = "namespace_id";
    }
    
    public static class ExternalPermissionMappings
    {
        public const string ExternalPermission = "external_permission";
        public const string InternalPermissionId = "permission_id";
        public const string NamespaceId = "namespace_id";
    }
    
    public static class DataSources
    {
        public const string Id = "id";
        public const string Name = "name";
        public const string TypeId = "type_id";
        public const string ConnectionSettings = "connection_string";
        public const string Purpose = "purpose";
    }
    
    public static class Dashboards
    {
        public const string Id = "id";
        public const string Name = "name";
        public const string Description = "description";
        public const string OwnerId = "owner_id";
        public const string CreatedAt = "created_at_utc";
        public const string UpdatedAt = "updated_at_utc";
        public const string NamespaceId = "namespace_id";
        public const string Tags = "tags";
    }

    public static class DashboardsUsersLink
    {
        public const string UserId = "id";
        public const string DashboardId = "dashboard_id";
    }

    public static class Panels
    {
        public const string Id = "id";
        public const string Title = "title";
        public const string TypeId = "type_id";
        public const string RawQuery = "raw_query";
        public const string DataSourceId = "data_source_id";
        public const string X = "x";
        public const string Y = "y";
        public const string Width = "width";
        public const string Height = "height";
        public const string DashboardId = "dashboard_id";
        public const string StylingConfiguration = "style_configuration";
    }

    public static class Alerts
    {
        public const string Id = "id";
        public const string Description = "description";
        public const string Template = "template";
        public const string Status = "status";
        public const string RawQuery = "condition_query";
        public const string DataSourceId = "data_source_id";
        public const string NotificationChannelGroupId = "notification_channel_group_id";
        public const string PreviousExecution = "previous_execution";
        public const string NextExecution = "next_execution";
        public const string WaitTimeBeforeAlertingInTicks = "wait_time_before_alerting_in_ticks";
        public const string RepeatIntervalInTicks = "repeat_interval_in_ticks";
        public const string Tags = "tags";
    }

    public static class NotificationChannels
    {
        public const string Id = "id";
        public const string DestinationId = "destination_id";
        public const string NotificationChannelGroupId = "notification_channel_group_id";
        public const string Settings = "settings";
    }
    
    public static class NotificationChannelGroups
    {
        public const string Id = "id";
        public const string Name = "name";
    }

    public static class DataSourceType
    {
        public const string Id = "id";
        public const string Name = "name";
    }

    public static class NotificationDestination
    {
        public const string Id = "id";
        public const string Name = "name";
    }

    public static class Variables
    {
        public const string Id = "id";
        public const string Placeholder = "placeholder";
        public const string Value = "value";
        public const string NamespaceId = "namespace_id";
        public const string DashboardId = "dashboard_id";
    }
}