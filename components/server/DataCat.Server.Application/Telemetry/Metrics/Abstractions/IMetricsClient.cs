namespace DataCat.Server.Application.Telemetry.Metrics.Abstractions;

public interface IMetricsClient : IDataSourceClient
{
    /// <summary>
    /// Executes a query and returns the matching metric points.
    /// </summary>
    /// <param name="query">The query expression.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Collection of metric points matching the query.</returns>
    Task<IEnumerable<MetricPoint>> QueryAsync(string query, CancellationToken token);

    /// <summary>
    /// Executes a range query over a specified time interval.
    /// </summary>
    /// <param name="query">The query expression.</param>
    /// <param name="start">Start time of the range.</param>
    /// <param name="end">End time of the range.</param>
    /// <param name="step">Resolution step between points (e.g., 1m, 5s).</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Time series data for the query over the range.</returns>
    Task<IEnumerable<TimeSeries>> RangeQueryAsync(string query, DateTime start, DateTime end, TimeSpan step, CancellationToken token);

    /// <summary>
    /// Lists available metric series that match a label selector.
    /// </summary>
    /// <param name="matchExpression">Label match expression (e.g., up{job="api"}).</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Collection of available metric series.</returns>
    Task<IEnumerable<MetricSeries>> ListSeriesAsync(string matchExpression, CancellationToken token);

    /// <summary>
    /// Gets the latest value of a metric.
    /// </summary>
    /// <param name="metricName">The name of the metric.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Latest metric value.</returns>
    Task<MetricPoint?> GetLatestMetricAsync(string metricName, CancellationToken token);

    /// <summary>
    /// Checks if an alert has been triggered based on a raw query.
    /// </summary>
    Task<bool> CheckAlertTriggerAsync(string rawQuery, CancellationToken token);
}