namespace DataCat.Server.Postgres;

public sealed class PostgresPlugin : IDatabasePlugin
{
    public void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IDefaultRepository<DashboardEntity, Guid>), typeof(DashboardRepository));
        services.AddScoped(typeof(IDefaultRepository<PluginEntity, Guid>), typeof(PluginRepository));
        services.AddScoped(typeof(IDefaultRepository<DataSourceEntity, Guid>), typeof(DataSourceRepository));
        services.AddScoped(typeof(IDefaultRepository<UserEntity, Guid>), typeof(UserRepository));

        services.AddScoped<ITogglePluginStatusRepository, TogglePluginStatusRepository>();
        
        services.AddSingleton<IMigrationRunnerFactory, PostgresRunnerFactory>(sp =>
        {
            var options = sp.GetRequiredService<DatabaseOptions>();
            return new PostgresRunnerFactory(options);
        });

        services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, PostgresConnectionFactory>();
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddSingleton<IDatabaseAssemblyScanner, DatabaseAssemblyScanner>();
    }
}