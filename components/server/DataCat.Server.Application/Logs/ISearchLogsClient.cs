namespace DataCat.Server.Application.Logs;

/// <summary>
/// Provides functionality for searching and analyzing log data in a log storage system.
/// </summary>
public interface ISearchLogsClient
{
    /// <summary>
    /// Searches for log entries matching the specified query criteria.
    /// </summary>
    /// <param name="query">The search criteria including filters, pagination and sorting options.</param>
    /// <param name="token">Cancellation token to abort the operation.</param>
    /// <returns>A task that represents the asynchronous operation and contains the search results.</returns>
    Task<LogSearchResult> SearchAsync(LogSearchQuery query, CancellationToken token = default);
    
    /// <summary>
    /// Retrieves distinct values for a specified field, optionally filtered by a base query.
    /// </summary>
    /// <param name="fieldName">The name of the field to get distinct values for.</param>
    /// <param name="baseQuery">Optional base query to filter the results before getting distinct values.</param>
    /// <param name="token">Cancellation token to abort the operation.</param>
    /// <returns>A task that represents the asynchronous operation and contains the collection of distinct values.</returns>
    Task<IReadOnlyCollection<string>> GetDistinctValuesAsync(
        string fieldName,
        LogSearchQuery? baseQuery = null,
        CancellationToken token = default
    );
    
    /// <summary>
    /// Gets a time-based summary of log entries aggregated by specified time intervals.
    /// </summary>
    /// <param name="query">The search criteria to filter logs.</param>
    /// <param name="interval">The time interval for aggregation (e.g., 1 hour, 5 minutes).</param>
    /// <param name="token">Cancellation token to abort the operation.</param>
    /// <returns>A task that represents the asynchronous operation and contains the time series summary.</returns>
    Task<TimeSeriesSummary> GetTimeSeriesSummaryAsync(
        LogSearchQuery query,
        TimeSpan interval,
        CancellationToken token = default
    );
    
    /// <summary>
    /// Ensures that the specified index exists in the storage. Creates the index if it does not exist.
    /// </summary>
    /// <param name="indexPattern">The name or pattern of the index to verify or create.</param>
    /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is <c>true</c> if the index already existed
    /// or was created successfully; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> CreateIndexIfNotExistsAsync(string indexPattern, CancellationToken token = default);
}