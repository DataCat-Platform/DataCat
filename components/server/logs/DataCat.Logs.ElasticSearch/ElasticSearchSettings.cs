namespace DataCat.Logs.ElasticSearch;

public sealed record ElasticSearchSettings(
    string ClusterUrl,
    string IndexPattern,
    TimeSpan RequestTimeout,
    string UserName,
    string Password) : ISearchLogsSettings;
