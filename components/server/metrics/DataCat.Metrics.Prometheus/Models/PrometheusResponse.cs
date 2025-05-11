namespace DataCat.Metrics.Prometheus.Models;

internal sealed record PrometheusResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;

    [JsonPropertyName("data")]
    public PrometheusData? Data { get; set; }
}