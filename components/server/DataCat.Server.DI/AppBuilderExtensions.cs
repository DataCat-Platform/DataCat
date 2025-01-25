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
    }
}