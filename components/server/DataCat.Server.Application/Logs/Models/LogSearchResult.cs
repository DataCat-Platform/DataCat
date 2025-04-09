namespace DataCat.Server.Application.Logs.Models;

/// <summary>
/// Result of search in searching system
/// </summary>
public sealed record LogSearchResult(
    IEnumerable<LogEntry> Entries,
    long TotalCount,
    int PageNumber,
    int PageSize);
