namespace DataCat.Server.Application.Telemetry.Metrics.Abstractions;

public interface IMetricsClientFactory
{
    bool CanCreate(DataSource dataSource);
    IMetricsClient CreateClient(DataSource dataSource);
}