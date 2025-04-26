namespace DataCat.Server.Application.Telemetry.Traces.Abstractions;

public interface ITracesClientFactory
{
    bool CanCreate(DataSource dataSource);
    ITracesClient CreateClient(DataSource dataSource);
}