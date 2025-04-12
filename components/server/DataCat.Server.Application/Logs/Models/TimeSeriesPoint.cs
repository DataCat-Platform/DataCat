namespace DataCat.Server.Application.Logs.Models;

/// <summary>
/// Represents a single time point in the time series aggregation.
/// </summary>
public sealed record TimeSeriesPoint(
    DateTime IntervalStart,
    DateTime IntervalEnd,
    long Count,
    Dictionary<string, long>? FacetCounts = null);
