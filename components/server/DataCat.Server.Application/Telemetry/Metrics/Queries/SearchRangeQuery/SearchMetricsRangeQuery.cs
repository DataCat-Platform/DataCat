namespace DataCat.Server.Application.Telemetry.Metrics.Queries.SearchRangeQuery;

public sealed record SearchMetricsRangeQuery(
    string DataSourceName,
    string Query,
    DateTime Start,
    DateTime End,
    TimeSpan Step,
    Guid? DashboardId = null) : IRequest<Result<IEnumerable<TimeSeries>>>, IAuthorizedQuery;