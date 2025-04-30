namespace DataCat.Server.Application.Telemetry;

public sealed class DataSourceManager(DataSourceContainer container, IServiceProvider serviceProvider)
{
    public IMetricsClient? GetMetricsClient(string dataSourceName)
    {
        var dataSource = container.Find(DataSourceKind.Metrics, dataSourceName);
        if (dataSource is null)
            return null;

        var factories = serviceProvider.GetServices<IMetricsClientFactory>();
        var factory = factories.FirstOrDefault(f => f.CanCreate(dataSource));
        return factory?.CreateClient(dataSource);
    }
    
    public ITracesClient? GetTracesClient(string dataSourceName)
    {
        var dataSource = container.Find(DataSourceKind.Traces, dataSourceName);
        if (dataSource is null)
            return null;

        var factories = serviceProvider.GetServices<ITracesClientFactory>();
        var factory = factories.FirstOrDefault(f => f.CanCreate(dataSource));
        return factory?.CreateClient(dataSource);
    }
    
    public ILogsClient? GetLogsClient(string dataSourceName)
    {
        var dataSource = container.Find(DataSourceKind.Logs, dataSourceName);
        if (dataSource is null)
            return null;

        var factories = serviceProvider.GetServices<ILogsClientFactory>();
        var factory = factories.FirstOrDefault(f => f.CanCreate(dataSource));
        return factory?.CreateClient(dataSource);
    }
}