namespace DataCat.Server.Metrics.Models;

public class ChartData
{
    public string Label { get; set; } = string.Empty;
    public List<int> Data { get; set; } = [];
}