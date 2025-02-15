namespace DataCat.Server.Metrics.Clients;

public interface IMetricsClient
{
    List<dynamic> GenerateMockMetrics();
}