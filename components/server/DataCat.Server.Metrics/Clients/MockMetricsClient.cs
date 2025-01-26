namespace DataCat.Server.Metrics.Clients;

public sealed class MockMetricsClient : IMetricsClient
{
    private static readonly Random _random = new();

    public List<dynamic> GenerateMockMetrics()
    {
        return
        [
            new { Label = "Metric A", Data = Enumerable.Range(0, 10).Select(_ => _random.Next(10, 100)).ToList() },
            new { Label = "Metric B", Data = Enumerable.Range(0, 10).Select(_ => _random.Next(20, 200)).ToList() },
            new { Label = "Metric C", Data = Enumerable.Range(0, 10).Select(_ => _random.Next(5, 50)).ToList() }
        ];
    }
}