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
        });

        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);

        services.Configure<PluginStoreOptions>(configuration.GetSection("PluginStoreOptions"));
        services.AddSingleton<PluginStoreOptions>(sp => sp.GetRequiredService<IOptions<PluginStoreOptions>>().Value);
        services.AddSingleton<IPluginStorage, DiskPluginStorage>();
        services.AddSingleton<DataSourceManager>();
        
        services.AddSingleton<IMetricClient, DataCatDbClient>(); // TODO: Register in another module
        
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

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
        services.AddSingleton<DatabaseOptions>(sp => sp.GetRequiredService<IOptions<DatabaseOptions>>().Value);
        
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
        
        string[] assemblyFiles = ["DataCat.Notifications.Email.dll", "DataCat.Notifications.Telegram.dll"];
        var pluginDirectory = AppContext.BaseDirectory;

        foreach (var notificationAssemblyName in assemblyFiles)
        {
            var assemblyFile = Path.Combine(pluginDirectory, notificationAssemblyName);
            
            if (!File.Exists(assemblyFile))
                throw new Exception($"Plugin assembly not found: {assemblyFile}");

            var assembly = Assembly.LoadFrom(assemblyFile);
            
            var pluginType = assembly.GetTypes()
                .FirstOrDefault(t => typeof(INotificationPlugin).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

            if (pluginType == null)
                throw new Exception($"No implementation of {nameof(INotificationPlugin)} found in {assemblyFile}");

            if (Activator.CreateInstance(pluginType) is INotificationPlugin plugin)
            {
                plugin.RegisterNotificationDestinationLibrary(services, configuration);
            }
            else
            {
                throw new Exception($"Failed to create an instance of {pluginType.FullName}");
            }
        }

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
}