namespace DataCat.Traces.Jaeger.Models;

internal sealed record JaegerTraceData
{
    [JsonPropertyName("traceID")]
    public string TraceId { get; init; } = string.Empty;

    [JsonPropertyName("spans")]
    public List<JaegerSpan> Spans { get; init; } = [];

    [JsonPropertyName("processes")]
    public Dictionary<string, JaegerProcess> Processes { get; init; } = [];

    [JsonPropertyName("warnings")]
    public List<string> Warnings { get; init; } = [];
}