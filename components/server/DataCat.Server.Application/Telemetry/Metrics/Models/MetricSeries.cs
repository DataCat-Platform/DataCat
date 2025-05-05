namespace DataCat.Server.Application.Telemetry.Metrics.Models;

/// <summary>
/// Represents a metric series (name + labels) without actual data points.
/// Useful for listing series.
/// </summary>
public sealed record MetricSeries
{
    public required string MetricName { get; init; }
    public IDictionary<string, string> Labels { get; init; } = new Dictionary<string, string>();
}