namespace DataCat.Server.Application.Metrics.Models;

public class MetricModel
{
    public required int MetricId { get; init; }
    public required double Value { get; init; }
    public required DateTime Timestamp { get; init; }
}