namespace DataCat.Metrics.Prometheus.Core;

public sealed class PrometheusClientFactory : IMetricsClientFactory
{
    public bool CanCreate(DataSource dataSource)
    {
        return dataSource.Purpose == DataSourcePurpose.Metrics
               && string.Compare(dataSource.DataSourceType.Name, PrometheusConstants.Prometheus, StringComparison.InvariantCultureIgnoreCase) == 0;
    }

    public IMetricsClient CreateClient(DataSource dataSource)
    {
        if (string.IsNullOrWhiteSpace(dataSource.ConnectionSettings))
        {
            throw new ArgumentException("ConnectionSettings must be provided for Prometheus DataSource", nameof(dataSource));
        }

        PrometheusSettings settings;
        try
        {
            settings = JsonSerializer.Deserialize<PrometheusSettings>(dataSource.ConnectionSettings)
                       ?? throw new InvalidOperationException("PrometheusSettings deserialized as null");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse PrometheusSettings from ConnectionSettings", ex);
        }
        
        var handler = new HttpClientHandler();
        var client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
        
        return new PrometheusClient(client, settings);
    }
}