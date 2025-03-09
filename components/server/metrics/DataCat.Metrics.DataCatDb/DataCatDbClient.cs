namespace DataCat.Metrics.DataCatDb;

public sealed class DataCatDbClient : IMetricClient
{
    private readonly Random _random = new Random();
    
    public DataSourceType DataSourceType { get; } = DataSourceType.DataCat;

    private readonly List<AlertModel> _alerts =
    [
        new()
        {
            AlertId = 1,
            AlertRawQuery = "SELECT COUNT(*) FROM metrics WHERE value > 100",
            AlertStatus = "triggered",
            AlertNextExecution = DateTime.UtcNow.AddMinutes(10),
            AlertWaitTimeBeforeAlertingInTicks = TimeSpan.FromMinutes(5).Ticks
        },
        new()
        {
            AlertId = 2,
            AlertRawQuery = "SELECT COUNT(*) FROM metrics WHERE value < 50",
            AlertStatus = "inactive",
            AlertNextExecution = DateTime.UtcNow.AddMinutes(5),
            AlertWaitTimeBeforeAlertingInTicks = TimeSpan.FromMinutes(3).Ticks
        }
    ];

    private readonly List<MetricModel> _metrics =
    [
        new() { MetricId = 1, Value = 150, Timestamp = DateTime.UtcNow.AddMinutes(-1) },
        new() { MetricId = 2, Value = 30, Timestamp = DateTime.UtcNow.AddMinutes(-5) },
        new() { MetricId = 3, Value = 200, Timestamp = DateTime.UtcNow.AddMinutes(-10) },
        new() { MetricId = 4, Value = 45, Timestamp = DateTime.UtcNow.AddMinutes(-15) }
    ];
    
    public Task<IEnumerable<MetricModel>> GetRandomMetricsAsync(int count, CancellationToken token)
    {
        var randomMetrics = _metrics.OrderBy(x => _random.Next()).Take(count);
        return Task.FromResult(randomMetrics);
    }

    public Task<bool> CheckAlertTriggerAsync(string rawQuery, CancellationToken token)
    {
        if (rawQuery.Contains("> 100"))
        {
            return Task.FromResult(_metrics.Any(m => m.Value > 100));
        }
        
        if (rawQuery.Contains("< 50"))
        {
            return Task.FromResult(_metrics.Any(m => m.Value < 50));
        }

        return Task.FromResult(false);
    }

    public Task<IEnumerable<AlertModel>> GetAlertsToCheckAsync(CancellationToken token)
    {
        var alertsToCheck = _alerts.Where(a => a.AlertStatus == "inactive" || a.AlertStatus == "triggered");
        return Task.FromResult(alertsToCheck);
    }
}
