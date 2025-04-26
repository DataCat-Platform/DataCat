namespace DataCat.Metrics.Prometheus;
//
// public static class DependencyInjection
// {
//     public static IServiceCollection AddPrometheusMetrics(
//         this IServiceCollection services,
//         IConfiguration configuration)
//     {
//         services.AddOptions<PrometheusMetricsSettings>()
//             .BindConfiguration(PrometheusMetricsSettings.ConfigurationSection)
//             .ValidateDataAnnotations()
//             .ValidateOnStart();
//         
//         services.AddSingleton<IMetricsSettings>(sp =>
//             sp.GetRequiredService<IOptions<PrometheusMetricsSettings>>().Value);
//
//         services.AddScoped<IMetricClient, ElasticSearchClientFactory>();
//         
//         return services;
//     }
// }