namespace DataCat.Metrics.Prometheus.Core;

public sealed class PrometheusClientFactory : IMetricsClientFactory
{
    public bool CanCreate(DataSource dataSource)
    {
        throw new NotImplementedException();
    }

    public IMetricsClient CreateClient(DataSource dataSource)
    {
        throw new NotImplementedException();
    }
}