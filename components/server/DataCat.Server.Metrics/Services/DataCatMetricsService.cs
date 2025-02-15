namespace DataCat.Server.Metrics.Services;

public sealed class DataCatMetricsService(IMetricsClient metricsClient) : IDataCatMetricsService
{
    public List<ChartData> GetMetrics()
    {
        var mockMetrics = metricsClient.GenerateMockMetrics();

        // Transform dynamic data into ChartData format
        return mockMetrics.Select(metric => new ChartData
        {
            Label = metric.Label,
            Data = metric.Data
        }).ToList();
    }
}