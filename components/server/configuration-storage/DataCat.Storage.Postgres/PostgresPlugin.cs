namespace DataCat.Storage.Postgres;

public sealed class PostgresPlugin : IDatabasePlugin
{
    public IServiceCollection RegisterRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IDefaultRepository<DashboardEntity, Guid>), typeof(DashboardRepository));
        services.AddScoped<IDashboardAccessRepository, DashboardAccessRepository>();
        services.AddScoped(typeof(IDefaultRepository<PluginEntity, Guid>), typeof(PluginRepository));
        services.AddScoped(typeof(IDefaultRepository<DataSourceEntity, Guid>), typeof(DataSourceRepository));
        services.AddScoped(typeof(IDefaultRepository<PanelEntity, Guid>), typeof(PanelRepository));
        services.AddScoped(typeof(IDefaultRepository<UserEntity, Guid>), typeof(UserRepository));
        services.AddScoped(typeof(IDefaultRepository<AlertEntity, Guid>), typeof(AlertRepository));
        services.AddScoped(typeof(IDefaultRepository<NotificationChannelEntity, Guid>), typeof(NotificationChannelRepository));
        services.AddScoped<ITogglePluginStatusRepository, TogglePluginStatusRepository>();
        
        services.AddSingleton<IMigrationRunnerFactory, PostgresRunnerFactory>(sp =>
        {
            var options = sp.GetRequiredService<DatabaseOptions>();
            return new PostgresRunnerFactory(options);
        });

        services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, PostgresConnectionFactory>();
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddSingleton<IDatabaseAssemblyScanner, DatabaseAssemblyScanner>();

        services.AddSingleton<IAlertMonitor, PostgresAlertMonitor>();
        services.AddSingleton<INotificationService, NotificationService>(); // TODO: Change to normal impl
        return services;
    }
}