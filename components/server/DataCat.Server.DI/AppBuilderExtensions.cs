namespace DataCat.Server.DI;

public static class AppBuilderExtensions
{
    public static async Task ApplyMigrations(
        this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbInitializer = scope.ServiceProvider.GetRequiredService<PostgresSeedService>();
        await dbInitializer.SeedAsync();
    }
}