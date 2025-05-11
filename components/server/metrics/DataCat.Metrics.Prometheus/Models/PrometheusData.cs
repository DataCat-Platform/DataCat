namespace DataCat.Metrics.Prometheus.Models;

internal sealed record PrometheusData
{
    [JsonPropertyName("resultType")]
    public string ResultType { get; set; } = null!;

    [JsonPropertyName("result")]
    public List<PrometheusResult> Result { get; set; } = [];
}