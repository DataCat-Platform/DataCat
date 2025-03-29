using DataCat.Server.Domain.Common;

namespace DataCat.Storage.Postgres;

public sealed class PostgresPlugin : IDatabasePlugin
{
    public IServiceCollection RegisterRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRepository<DashboardEntity, Guid>, DashboardRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        
        services.AddScoped<IRepository<DataSourceEntity, Guid>, DataSourceRepository>();
        services.AddScoped<IDataSourceRepository, DataSourceRepository>();

        services.AddScoped<IRepository<PanelEntity, Guid>, PanelRepository>();
        services.AddScoped<IPanelRepository, PanelRepository>();

        services.AddScoped<IRepository<UserEntity, Guid>, UserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IRepository<AlertEntity, Guid>, AlertRepository>();
        services.AddScoped<IAlertRepository, AlertRepository>();

        services.AddScoped<IRepository<NotificationChannelEntity, Guid>, NotificationChannelRepository>();
        services.AddScoped<INotificationChannelRepository, NotificationChannelRepository>();
        
        services.AddScoped<IRepository<PluginEntity, Guid>, PluginRepository>();
        services.AddScoped<IPluginRepository, PluginRepository>();
        
        services.AddScoped<UnitOfWork>();
        services.AddScoped<IUnitOfWork<NpgsqlTransaction>>(provider => provider.GetRequiredService<UnitOfWork>());
        
        services.AddSingleton<IMigrationRunnerFactory, PostgresRunnerFactory>(sp =>
        {
            var options = sp.GetRequiredService<DatabaseOptions>();
            return new PostgresRunnerFactory(options);
        });

        services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, PostgresConnectionFactory>();
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddSingleton<IDatabaseAssemblyScanner, DatabaseAssemblyScanner>();

        services.AddScoped<IAlertMonitorService, PostgresAlertMonitorService>();

        AddApiBackgroundWorkers(services);
        AddPipelines(services);
        
        return services;
    }


    private static IServiceCollection AddApiBackgroundWorkers(IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            #region AlertChecker
            var alertCheckerKey = new JobKey("AlertChecker");
            q.AddJob<AlertChecker>(opts => opts.WithIdentity(alertCheckerKey));
    
            q.AddTrigger(opts => opts
                .ForJob(alertCheckerKey)
                .WithIdentity("AlertChecker-trigger")
                .WithSimpleSchedule(action =>
                {
                    action.WithIntervalInSeconds(5).RepeatForever();
                })
            );
            #endregion
            
            #region AlertNotifier
            var alertNotifierKey = new JobKey("AlertNotifier");
            q.AddJob<AlertNotifier>(opts => opts.WithIdentity(alertNotifierKey));
            q.AddTrigger(opts => opts
                .ForJob(alertNotifierKey)
                .WithIdentity("AlertNotifier-trigger")
                .WithSimpleSchedule(action =>
                {
                    action.WithIntervalInSeconds(20).RepeatForever();
                })
            );
            #endregion
        });
        
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }
    
    private static IServiceCollection AddPipelines(IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionScopeBehavior<,>));
        return services;
    }
}