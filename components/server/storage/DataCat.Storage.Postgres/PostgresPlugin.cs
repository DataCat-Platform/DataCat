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