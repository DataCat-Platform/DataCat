using DataCat.Server.Domain.Core;

namespace DataCat.Logs.ElasticSearch.Searching;

public sealed class ElasticClientFactory : ILogsClientFactory, IDisposable
{
    private static readonly JsonSerializerOptions serializationOptions = new() { PropertyNameCaseInsensitive = true };
    
    public bool CanCreate(DataSource dataSource)
    {
        return dataSource.Purpose == DataSourcePurpose.Logs
               && string.Compare(dataSource.DataSourceType.Name, ElasticSearchConstants.ElasticSearch, StringComparison.InvariantCultureIgnoreCase) == 0;
    }

    public ILogsClient CreateClient(DataSource dataSource)
    {
        if (string.IsNullOrWhiteSpace(dataSource.ConnectionSettings))
        {
            throw new ArgumentException("ConnectionSettings must be provided for ElasticSearch DataSource", nameof(dataSource));
        }

        ElasticSettings settings;
        try
        {
            settings = JsonSerializer.Deserialize<ElasticSettings>(dataSource.ConnectionSettings, serializationOptions)
                       ?? throw new InvalidOperationException("ElasticSettings deserialized as null");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse ElasticSettings from ConnectionSettings", ex);
        }

        var nodes = new[] { new Uri(settings.ClusterUrl) };
        var pool = new StaticNodePool(nodes);

        var config = new ElasticsearchClientSettings(pool)
            // .Authentication(new BasicAuthentication(settings.UserName, settings.Password))
            .RequestTimeout(TimeSpan.FromSeconds(15))
            .EnableDebugMode(settings.EnableDebugLogging ? OnRequestCompleted : null!);

        var elasticClient = new ElasticsearchClient(config);

        return new ElasticClient(elasticClient, settings.IndexPattern);
    }
    
    private static void OnRequestCompleted(ApiCallDetails details)
    {
        Console.WriteLine(details.DebugInformation);
    }
    
    public void Dispose() {}
}