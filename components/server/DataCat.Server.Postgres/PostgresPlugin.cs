namespace DataCat.Server.Postgres;

public sealed class PostgresPlugin : IDatabasePlugin
{
    public void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IDefaultRepository<PluginEntity, Guid>), typeof(PluginDefaultRepository));
        services.AddScoped<ITogglePluginStatusRepository, TogglePluginStatusRepository>();
        
        services.AddSingleton<IMigrationRunnerFactory, PostgresRunnerFactory>(sp =>
        {
            var options = sp.GetRequiredService<DatabaseOptions>();
            return new PostgresRunnerFactory(options);
        });

        services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, PostgresConnectionFactory>();
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}