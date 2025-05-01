namespace DataCat.Server.Application.Telemetry.Traces.Models;

public sealed record SpanReference
{
    public required string TraceId { get; init; }
    public required string SpanId { get; init; }
    public required SpanReferenceType ReferenceType { get; init; }
}