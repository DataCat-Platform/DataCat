namespace DataCat.Server.Postgres.Migrations;

[Migration(2)]
public sealed class CreateUsersTable : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static CreateUsersTable()
    {
        UpSql = @$"
            CREATE TABLE {Public.UserTable} (
                {Public.Users.Id} TEXT PRIMARY KEY,
                {Public.Users.IdentityId} TEXT NOT NULL UNIQUE,
                {Public.Users.Email} TEXT NULL UNIQUE,
                {Public.Users.Name} TEXT NULL,
                {Public.Users.CreatedAt} TIMESTAMP NOT NULL,
                {Public.Users.UpdatedAt} TIMESTAMP NULL
            );

            CREATE TABLE {Public.RoleTable} (
                {Public.Roles.Id} INTEGER PRIMARY KEY,
                {Public.Roles.Name} VARCHAR(100) UNIQUE
            );

            CREATE TABLE {Public.PermissionsTable} (
                {Public.Permissions.Id} SERIAL PRIMARY KEY,
                {Public.Permissions.Name} VARCHAR(100) UNIQUE
            );

            CREATE TABLE {Public.NamespaceTable} (
                {Public.Namespaces.Id} TEXT PRIMARY KEY,
                {Public.Namespaces.Name} VARCHAR(255) UNIQUE
            );

            CREATE TABLE {Public.UserRoleLinkTable} (
                {Public.UsersRolesLink.UserId} TEXT NOT NULL,
                {Public.UsersRolesLink.RoleId} INTEGER NOT NULL,
                {Public.UsersRolesLink.NamespaceId} TEXT NOT NULL,
                {Public.UsersRolesLink.IsManual} BOOLEAN DEFAULT FALSE,
                PRIMARY KEY ({Public.UsersRolesLink.UserId}, {Public.UsersRolesLink.RoleId}, {Public.UsersRolesLink.NamespaceId}),
                FOREIGN KEY ({Public.UsersRolesLink.UserId}) REFERENCES {Public.UserTable}({Public.Users.Id}) ON DELETE CASCADE,
                FOREIGN KEY ({Public.UsersRolesLink.RoleId}) REFERENCES {Public.RoleTable}({Public.Roles.Id}) ON DELETE CASCADE,
                FOREIGN KEY ({Public.UsersRolesLink.NamespaceId}) REFERENCES {Public.NamespaceTable}({Public.Namespaces.Id}) ON DELETE CASCADE
            );

            CREATE TABLE {Public.UserPermissionLinkTable} (
                {Public.UsersPermissionsLink.UserId} TEXT NOT NULL,
                {Public.UsersPermissionsLink.PermissionId} INTEGER NOT NULL,
                {Public.UsersPermissionsLink.IsManual} BOOLEAN DEFAULT FALSE,
                {Public.UsersPermissionsLink.NamespaceId} TEXT NOT NULL,
                PRIMARY KEY ({Public.UsersPermissionsLink.UserId}, {Public.UsersPermissionsLink.PermissionId}, {Public.UsersPermissionsLink.NamespaceId}),
                FOREIGN KEY ({Public.UsersPermissionsLink.UserId}) REFERENCES {Public.UserTable}({Public.Users.Id}) ON DELETE CASCADE,
                FOREIGN KEY ({Public.UsersPermissionsLink.PermissionId}) REFERENCES {Public.RoleTable}({Public.Roles.Id}) ON DELETE CASCADE,
                FOREIGN KEY ({Public.UsersPermissionsLink.NamespaceId}) REFERENCES {Public.NamespaceTable}({Public.Namespaces.Id}) ON DELETE CASCADE
            );

            CREATE TABLE {Public.ExternalRoleMappingTable} (
                {Public.ExternalRoleMappings.ExternalRole} TEXT NOT NULL,
                {Public.ExternalRoleMappings.NamespaceId} TEXT NOT NULL,
                {Public.ExternalRoleMappings.InternalRoleId} INTEGER NOT NULL,
                PRIMARY KEY ({Public.ExternalRoleMappings.ExternalRole}, {Public.ExternalRoleMappings.NamespaceId}),
                FOREIGN KEY ({Public.ExternalRoleMappings.InternalRoleId}) REFERENCES {Public.RoleTable}({Public.Roles.Id}) ON DELETE CASCADE,
                FOREIGN KEY ({Public.ExternalRoleMappings.NamespaceId}) REFERENCES {Public.NamespaceTable}({Public.Namespaces.Id}) ON DELETE CASCADE
            );

            CREATE TABLE {Public.ExternalPermissionMappingTable} (
                {Public.ExternalPermissionMappings.ExternalPermission} TEXT NOT NULL,
                {Public.ExternalPermissionMappings.NamespaceId} TEXT NOT NULL,
                {Public.ExternalPermissionMappings.InternalPermissionId} INTEGER NOT NULL,
                PRIMARY KEY ({Public.ExternalPermissionMappings.ExternalPermission}, {Public.ExternalPermissionMappings.NamespaceId}),
                FOREIGN KEY ({Public.ExternalPermissionMappings.InternalPermissionId}) REFERENCES {Public.RoleTable}({Public.Roles.Id}) ON DELETE CASCADE,
                FOREIGN KEY ({Public.ExternalPermissionMappings.NamespaceId}) REFERENCES {Public.NamespaceTable}({Public.Namespaces.Id}) ON DELETE CASCADE
            );
        ";
        
        DownSql = $@"
            DROP TABLE IF EXISTS {Public.ExternalPermissionMappingTable};
            DROP TABLE IF EXISTS {Public.ExternalRoleMappingTable};
            DROP TABLE IF EXISTS {Public.UserPermissionLinkTable};
            DROP TABLE IF EXISTS {Public.UserRoleLinkTable};
            DROP TABLE IF EXISTS {Public.NamespaceTable};
            DROP TABLE IF EXISTS {Public.PermissionsTable};
            DROP TABLE IF EXISTS {Public.RoleTable};
            DROP TABLE IF EXISTS {Public.UserTable};
        ";
    }
    
    public override void Up()
    {
        Execute.Sql(UpSql);
    }

    public override void Down()
    {
        Execute.Sql(DownSql);
    }
}