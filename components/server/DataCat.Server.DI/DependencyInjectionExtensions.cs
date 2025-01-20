namespace DataCat.Server.DI;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApiSetup(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<AddPluginCommandHandler>();

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);

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
        
        switch (configuration["DataSourceType"])
        {
            case "sqlserver":
                break;
            case "postgres":
                services.Configure<MigrationOptions>(configuration.GetSection("MigrationOptions"));
                services.AddSingleton<MigrationOptions>(sp => sp.GetRequiredService<IOptions<MigrationOptions>>().Value);
                services.AddSingleton<IMigrationRunnerFactory, PostgresRunnerFactory>(sp =>
                {
                    var options = sp.GetRequiredService<MigrationOptions>();
                    return new PostgresRunnerFactory(options);
                });
                break;
        }

        return services;
    }
}