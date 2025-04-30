namespace DataCat.Server.Application.Telemetry.Metrics.Queries.SearchRangeQuery;

public sealed record SearchMetricsRangeQuery(
    string DataSourceName,
    string Query,
    DateTime Start,
    DateTime End,
    TimeSpan Step) : IRequest<Result<IEnumerable<TimeSeries>>>, IAuthorizedQuery;