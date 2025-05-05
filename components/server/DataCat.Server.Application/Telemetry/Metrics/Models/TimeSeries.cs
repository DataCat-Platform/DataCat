namespace DataCat.Server.Application.Telemetry.Metrics.Models;

/// <summary>
/// Represents a time series of metric points.
/// </summary>
public sealed record TimeSeries
{
    public required string MetricName { get; init; }
    public IDictionary<string, string> Labels { get; init; } = new Dictionary<string, string>();
    public List<MetricPoint> Points { get; init; } = [];
}