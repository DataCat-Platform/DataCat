namespace DataCat.Server.Application.Logs.Models;

/// <summary>
/// Represents a time-based aggregation of log entries.
/// </summary>
/// <param name="IntervalStart">The start time of the aggregation interval.</param>
/// <param name="IntervalEnd">The end time of the aggregation interval.</param>
/// <param name="Count">The total number of log entries in this interval.</param>
/// <param name="FacetCounts">Optional dictionary containing counts for specific facets/categories within the interval.</param>
public sealed record TimeSeriesSummary(
    DateTime IntervalStart,
    DateTime IntervalEnd,
    long Count,
    Dictionary<string, long>? FacetCounts = null
);