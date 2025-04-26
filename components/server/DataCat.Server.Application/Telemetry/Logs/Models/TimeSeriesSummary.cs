namespace DataCat.Server.Application.Telemetry.Logs.Models;

/// <summary>
/// Represents a time-based aggregation of log entries.
/// </summary>
public sealed record TimeSeriesSummary(List<TimeSeriesPoint> Points);