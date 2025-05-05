namespace DataCat.Traces.Jaeger.Models;

internal sealed record JaegerSpan
{
    [JsonPropertyName("traceID")]
    public string TraceId { get; init; } = string.Empty;

    [JsonPropertyName("spanID")]
    public string SpanId { get; init; } = string.Empty;

    [JsonPropertyName("operationName")]
    public string OperationName { get; init; } = string.Empty;

    [JsonPropertyName("references")]
    public List<JaegerSpanReference> References { get; init; } = [];

    [JsonPropertyName("startTime")]
    public long StartTime { get; init; }

    [JsonPropertyName("duration")]
    public long Duration { get; init; }

    [JsonPropertyName("tags")]
    public List<JaegerTag> Tags { get; init; } = [];

    [JsonPropertyName("processID")]
    public string ProcessId { get; init; } = string.Empty;

    [JsonPropertyName("warnings")]
    public List<string> Warnings { get; init; } = [];
}