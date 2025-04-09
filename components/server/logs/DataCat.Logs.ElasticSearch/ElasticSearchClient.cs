namespace DataCat.Logs.ElasticSearch;

public sealed class ElasticSearchClient : ISearchLogsClient
{
    public Task<LogSearchResult> SearchAsync(LogSearchQuery query, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<string>> GetDistinctValuesAsync(string fieldName, LogSearchQuery? baseQuery = null, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<TimeSeriesSummary> GetTimeSeriesSummaryAsync(LogSearchQuery query, TimeSpan interval, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateIndexIfNotExistsAsync(string indexPattern, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}