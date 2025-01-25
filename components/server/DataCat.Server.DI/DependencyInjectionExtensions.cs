using System.Text.Json;
using DataCat.Server.Application.Behaviors.TransactionScope;

namespace DataCat.Server.DI;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApiSetup(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
            });;
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<AddPluginCommandHandler>();

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
        });

        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);

        services.Configure<PluginStoreOptions>(configuration.GetSection("PluginStoreOptions"));
        services.AddSingleton<PluginStoreOptions>(sp => sp.GetRequiredService<IOptions<PluginStoreOptions>>().Value);
        services.AddSingleton<IPluginStorage, DiskPluginStorage>();

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
}