namespace DataCat.Metrics.Prometheus.Models;

internal sealed record PrometheusResult
{
    [JsonPropertyName("metric")]
    public Dictionary<string, string> Metric { get; set; } = new();

    [JsonPropertyName("value")]
    public object[] Value { get; set; } = [];

    [JsonPropertyName("values")]
    public List<object[]> Values { get; set; } = [];
}