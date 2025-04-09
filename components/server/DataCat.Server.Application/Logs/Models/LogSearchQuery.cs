namespace DataCat.Server.Application.Logs.Models;

// Параметры поиска логов
public sealed record LogSearchQuery(
    string? TraceId = null,
    DateTime? From = null,
    DateTime? To = null,
    string? Severity = null,
    string? ServiceName = null,
    Dictionary<string, string>? CustomFilters = null,
    int PageSize = 100,
    int PageNumber = 1,
    string? SortField = LogSortFields.Timestamp,
    bool SortAscending = false
);