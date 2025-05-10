namespace DataCat.Server.Application.Queries.Alerts.GetCounters;

public sealed record AlertCounterResponse
{
    public required string Status { get; init; }
    public required long Count { get; init; }
}