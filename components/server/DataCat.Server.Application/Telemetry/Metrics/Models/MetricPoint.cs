namespace DataCat.Server.Application.Telemetry.Metrics.Models;

/// <summary>
/// Represents a single metric value at a point in time.
/// </summary>
public class MetricPoint
{
    public required string MetricName { get; init; }
    public required double Value { get; init; }
    public required DateTime Timestamp { get; init; }
    public IDictionary<string, string> Labels { get; init; } = new Dictionary<string, string>();
}