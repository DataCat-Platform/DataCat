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
}