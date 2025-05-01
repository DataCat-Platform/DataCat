namespace DataCat.Traces.Jaeger;

public static class DependencyInjection
{
    public static IServiceCollection AddJaegerTraces(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ITracesClientFactory, JaegerClientFactory>();
        services.AddHostedService<JaegerInitializer>();
        
        return services;
    }
}