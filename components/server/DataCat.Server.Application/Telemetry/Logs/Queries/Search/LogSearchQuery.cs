namespace DataCat.Server.Application.Telemetry.Logs.Queries.Search;

public sealed record LogSearchQuery(
    string DataSourceName,
    string? TraceId = null,
    DateTime? From = null,
    DateTime? To = null,
    string? Severity = null,
    string? ServiceName = null,
    Dictionary<string, string>? CustomFilters = null,
    int PageSize = 100,
    int Page = 1,
    string? SortField = null,
    bool SortAscending = false
) : IRequest<Result<Page<LogEntry>>>, IPaginationQuery;