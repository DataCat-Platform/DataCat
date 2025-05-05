namespace DataCat.Metrics.Prometheus.Models;

internal sealed record PrometheusSeriesResponse
{
    [JsonPropertyName("data")]
    public List<Dictionary<string, string>> Data { get; set; } = [];
}