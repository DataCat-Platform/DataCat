namespace DataCat.Server.Application.Metrics;

public sealed class DataSourceManager(IEnumerable<IMetricClient> metricClients)
{
    private readonly Dictionary<string, IMetricClient> _clientCache = new();

    public IMetricClient GetMetricClient(DataSourceEntity dataSource)
    {
        var key = dataSource.Name;

        if (_clientCache.TryGetValue(key, out var client))
        {
            return client;
        }

        var metricClient = metricClients.FirstOrDefault(x => x.DataSourceType == dataSource.DataSourceType);
        _clientCache[key] = metricClient 
                            ?? throw new InvalidOperationException($"The data source type {dataSource.DataSourceType} is not supported.");

        return metricClient;
    }
}