namespace DataCat.Server.DI;

public static class AppBuilderExtensions
{
    public static async Task ApplyMigrations(
        this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var migrationFactory = services.GetRequiredService<IMigrationRunnerFactory>();
        var runner = migrationFactory.CreateMigrationRunner();
        await runner.ApplyMigrationsAsync();

        var databaseOptions = services.GetRequiredService<DatabaseOptions>();

        await SeedRolesAsync(databaseOptions.ConnectionString);
        await SeedPermissionsAsync(databaseOptions.ConnectionString);
        await SeedDefaultNamespace(databaseOptions.ConnectionString);
    }
    
    private static async Task SeedRolesAsync(string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        const string rolesExistQuery = $"SELECT COUNT(*) FROM {Public.RoleTable};";
        var rolesCount = await connection.ExecuteScalarAsync<long>(rolesExistQuery);

        if (rolesCount == 0)
        {
            var roles = UserRole.All
                .Select(r => new { name = r.Name, id = r.Value })
                .ToList();

            foreach (var role in roles)
            {
                const string insertRoleQuery = $@"
                    INSERT INTO {Public.RoleTable} ({Public.Roles.Id}, {Public.Roles.Name}) 
                    VALUES (@id, @name);";

                await using var insertRoleCommand = new NpgsqlCommand(insertRoleQuery, connection);
                insertRoleCommand.Parameters.AddWithValue("@id", role.id);
                insertRoleCommand.Parameters.AddWithValue("@name", role.name);

                await insertRoleCommand.ExecuteNonQueryAsync();
            }
        }
    }

    private static async Task SeedPermissionsAsync(string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        const string permissionsExistQuery = $"SELECT COUNT(*) FROM {Public.PermissionsTable};";
        var permissionsCount = await connection.ExecuteScalarAsync<long>(permissionsExistQuery);

        if (permissionsCount == 0)
        {
            var permissions = UserPermission.All
                .Select(r => new { name = r.Name, id = r.Value })
                .ToList();

            foreach (var permission in permissions)
            {
                const string insertPermissionQuery = $@"
                    INSERT INTO {Public.PermissionsTable} ({Public.Permissions.Id}, {Public.Permissions.Name}) 
                    VALUES (@id, @name);";

                await using var insertPermissionCommand = new NpgsqlCommand(insertPermissionQuery, connection);
                insertPermissionCommand.Parameters.AddWithValue("@id", permission.id);
                insertPermissionCommand.Parameters.AddWithValue("@name", permission.name);

                await insertPermissionCommand.ExecuteNonQueryAsync();
            }
        }
    }
    
    private static async Task SeedDefaultNamespace(string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        const string defaultNamespaceExistsQuery = $"""
            SELECT 1 
            FROM {Public.NamespaceTable} n 
            WHERE n.{Public.Namespaces.Name} = '{ApplicationConstants.DefaultNamespace}';
        """;
        var isExist = await connection.ExecuteScalarAsync<long>(defaultNamespaceExistsQuery);

        if (isExist != 1)
        {
            var insertSql = $"""
                INSERT INTO {Public.NamespaceTable} ({Public.Namespaces.Id}, {Public.Namespaces.Name})
                VALUES ('{Guid.NewGuid()}', '{ApplicationConstants.DefaultNamespace}');
            """; 
            
            await connection.ExecuteAsync(insertSql);
        }
    }
}