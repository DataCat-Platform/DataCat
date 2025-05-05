using DataCat.Notifications.Webhook;

namespace DataCat.Server.DI;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<AddPluginCommandHandler>();
            
            config.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(CommandMetricsBehavior<,>));
            config.AddOpenBehavior(typeof(QueryMetricsBehavior<,>));
        });

        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);

        services.Configure<PluginStoreOptions>(configuration.GetSection("PluginStoreOptions"));
        services.AddSingleton<PluginStoreOptions>(sp => sp.GetRequiredService<IOptions<PluginStoreOptions>>().Value);
        services.AddSingleton<IPluginStorage, DiskPluginStorage>();
        
        services.AddSingleton<DataSourceManager>();
        services.AddSingleton<DataSourceContainer>();

        services.AddScoped<INamespaceService, NamespaceService>();
        services.AddScoped<IVariableService, VariableService>();
        
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
        services.AddQuartz(q =>
        {
            #region DataSourceContainerLoaderJob
            var dataSourceContainerLoaderJob = new JobKey("DataSourceContainerLoaderJob");
            q.AddJob<DataSourceContainerLoaderJob>(opts => opts.WithIdentity(dataSourceContainerLoaderJob));
    
            q.AddTrigger(opts => opts
                .ForJob(dataSourceContainerLoaderJob)
                .WithIdentity("DataSourceContainerLoaderJob-trigger")
                .WithSimpleSchedule(action =>
                {
                    action.WithIntervalInMinutes(60).RepeatForever();
                })
            );
            #endregion
        });

        return services;
    }

    public static IServiceCollection AddServerLogging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .WriteTo.Console()
            .CreateLogger()));

        return services;
    }
    
    public static IServiceCollection AddMigrationSetup(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = bool.TryParse(configuration["ApplyMigrations"], out var applyMigrations);
        if (!applyMigrations)
        {
            return services;    
        }
        
        NullGuard.ThrowIfNullOrWhiteSpace(configuration["DataSourceType"]);
        
        services.Configure<DatabaseOptions>(configuration.GetSection("DatabaseOptions"));
        services.AddSingleton<DatabaseOptions>(sp =>
        {
            var option = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            var secretPath = option.ConnectionString;

            var secretsProvider = sp.GetRequiredService<ISecretsProvider>();
            
            var connectionString = secretsProvider.GetSecretAsync(secretPath).GetAwaiter().GetResult();
            
            return option with { ConnectionString = connectionString };
        });
        
        PluginLoader.LoadDatabasePlugin(services, configuration["DataSourceType"]!, configuration);

        return services;
    }
    
    public static IServiceCollection AddSecretsSetup(
        this IServiceCollection services,
        IConfiguration configuration) 
    {
        NullGuard.ThrowIfNullOrWhiteSpace(configuration["SecretsStorageType"]);
        PluginLoader.LoadSecretStoragePlugin(services, configuration["SecretsStorageType"]!, configuration);
        return services;
    }

    public static IServiceCollection AddAuthSetup(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        NullGuard.ThrowIfNullOrWhiteSpace(configuration["AuthType"]);
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.Configure<AuthMappingOptions>(configuration.GetSection("AuthMappingOptions"));
        services.AddSingleton<AuthMappingOptions>(sp => sp.GetRequiredService<IOptions<AuthMappingOptions>>().Value);
        PluginLoader.LoadAuthPlugin(services, configuration["AuthType"]!, configuration);
        return services;
    }

    public static IServiceCollection AddNotificationsSetup(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<NotificationChannelManager>();

        var telegramPlugin = new TelegramPlugin();
        var emailPlugin = new EmailPlugin();
        var webhookPlugin = new WebhookPlugin();
        
        telegramPlugin.RegisterNotificationDestinationLibrary(services, configuration);
        emailPlugin.RegisterNotificationDestinationLibrary(services, configuration);
        webhookPlugin.RegisterNotificationDestinationLibrary(services, configuration);
        
        return services;
    }

    public static IServiceCollection AddRealTimeCommunication(
        this IServiceCollection services,
        IConfiguration _)
    {
        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
        });
        
        services.AddCors(corsOptions =>
        {
            corsOptions.AddPolicy("frontend",x =>
                x.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });
        
        return services;
    }

    public static IServiceCollection AddSearchLogsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddElasticSearchLogSearching(configuration);
    }
    
    public static IServiceCollection AddSearchMetricsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddPrometheusMetrics(configuration);
    }
    
    public static IServiceCollection AddSearchTracesServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddJaegerTraces(configuration);
    }

    public static IServiceCollection AddCachingServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache();

        services.AddRedisCaching(configuration);
        
        
        return services;
    }

    public static IServiceCollection AddObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        ILoggingBuilder loggingBuilder)
    {
        services.AddTelemetry(configuration, loggingBuilder);
        services.AddSingleton<IMetricsContainer, MetricsContainer>();
        return services;
    }
}