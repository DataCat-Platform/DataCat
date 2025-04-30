namespace DataCat.Metrics.Prometheus;

public static class DependencyInjection
{
    public static IServiceCollection AddPrometheusMetrics(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IMetricsClientFactory, PrometheusClientFactory>();
        services.AddHostedService<PrometheusInitializer>();
        
        return services;
    }
}