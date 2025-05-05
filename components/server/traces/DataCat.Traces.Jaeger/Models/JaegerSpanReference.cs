namespace DataCat.Traces.Jaeger.Models;

internal sealed record JaegerSpanReference
{
    [JsonPropertyName("traceID")]
    public string TraceId { get; init; } = string.Empty;

    [JsonPropertyName("spanID")]
    public string SpanId { get; init; } = string.Empty;

    [JsonPropertyName("refType")]
    public string RefType { get; init; } = string.Empty;
}