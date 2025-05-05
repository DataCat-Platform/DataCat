namespace DataCat.Server.Application.Telemetry.Traces.Models;

public sealed record SpanEntry
{
    public required string TraceId { get; init; }
    public required string SpanId { get; init; }
    public required string OperationName { get; init; }
    public required DateTime StartTime { get; init; }
    public required TimeSpan Duration { get; init; }
    public Dictionary<string, object> Tags { get; init; } = new();
    public List<SpanReference> References { get; init; } = [];
    public required string ProcessId { get; init; }
}