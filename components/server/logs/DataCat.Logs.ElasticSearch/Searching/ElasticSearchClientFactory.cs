namespace DataCat.Logs.ElasticSearch.Searching;

public sealed class ElasticSearchClientFactory : ISearchLogsClientFactory, IDisposable
{
    private readonly ElasticsearchClient _client;
    private readonly string _defaultIndexPattern;

    public ElasticSearchClientFactory(ISearchLogsSettings settings)
    {
        var nodes = new[] { new Uri(settings.ClusterUrl) };
        var pool = new StaticNodePool(nodes);

        var config = new ElasticsearchClientSettings(pool)
            // .Authentication(new BasicAuthentication(settings.UserName, settings.Password))
            .RequestTimeout(settings.RequestTimeout)
            .EnableDebugMode(settings.EnableDebugLogging ? OnRequestCompleted : null!);

        _client = new ElasticsearchClient(config);
        _defaultIndexPattern = settings.IndexPattern;
    }
    
    private static void OnRequestCompleted(ApiCallDetails details)
    {
        Console.WriteLine(details.DebugInformation);
    }

    public ISearchLogsClient CreateClient() => new ElasticSearchClient(_client, _defaultIndexPattern);

    public ISearchLogsClient CreateClient(string indexPattern) => new ElasticSearchClient(_client, indexPattern);

    public void Dispose() {}
}