namespace DataCat.Server.Application.Telemetry.Traces.Models;

public sealed record ProcessEntry
{
    public required string ServiceName { get; init; }
    public Dictionary<string, object> Tags { get; init; } = new();
}