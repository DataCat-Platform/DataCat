namespace DataCat.Server.Application.Telemetry.Metrics.Queries.SearchQuery;

public sealed record SearchMetricsQuery(
    string DataSourceName,
    string Query) : IRequest<Result<IEnumerable<MetricPoint>>>, IAuthorizedQuery;
