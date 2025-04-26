namespace DataCat.Metrics.Prometheus.Models;

public sealed class PrometheusMetricsSettings : IMetricsSettings
{
    public required string BaseUrl { get; init; }
} 
