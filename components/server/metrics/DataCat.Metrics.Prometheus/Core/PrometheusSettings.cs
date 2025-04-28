namespace DataCat.Metrics.Prometheus.Core;

public sealed class PrometheusSettings
{
    public required string ServerUrl { get; set; } = null!;

    public string? AuthToken { get; set; }
}