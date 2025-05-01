namespace DataCat.Server.Application.Telemetry.Traces.Queries.GetTrace;

public sealed record GetTraceQuery(
    string DataSourceName,
    string TraceId) : IRequest<Result<TraceEntry?>>, IAuthorizedQuery;