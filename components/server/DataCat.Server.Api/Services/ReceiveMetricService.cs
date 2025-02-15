namespace DataCat.Server.Api.Services;

public class ReceiveMetricService : DataCatMetricExporter.DataCatMetricExporterBase
{
    public override async Task<Empty> SendMetrics(SendMetricsRequest request, ServerCallContext context)
    {
        Console.WriteLine($"[{DateTime.UtcNow}] Sending metrics to server. Count of metrics: {request.Metrics.Count}]");
        foreach (var metric in request.Metrics)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] Received metric: {JsonConvert.SerializeObject(metric)}");
        }
        await Task.CompletedTask;
        return new Empty();
    }
}