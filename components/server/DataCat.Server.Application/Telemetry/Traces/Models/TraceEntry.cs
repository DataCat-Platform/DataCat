namespace DataCat.Server.Application.Telemetry.Traces.Models;

public sealed record TraceEntry
{
    public required string TraceId { get; init; }
    public List<SpanEntry> Spans { get; init; } = [];
    public Dictionary<string, ProcessEntry> Processes { get; init; } = new();
}