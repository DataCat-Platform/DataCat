namespace DataCat.Server.Metrics.Services;

public interface IDataCatMetricsService
{
    List<ChartData> GetMetrics();
}