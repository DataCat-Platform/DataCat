namespace DataCat.Server.Application.Telemetry.Traces.Queries.Search;

public sealed record SearchTracesQuery(
    string DataSourceName,
    string Service,
    DateTime Start,
    DateTime End,
    string? Operation = null,
    int? Limit = null,
    string? MinDuration = null,
    string? MaxDuration = null,
    Dictionary<string, object>? Tags = null) 
    : IRequest<Result<IEnumerable<TraceEntry>>>, IAuthorizedQuery;