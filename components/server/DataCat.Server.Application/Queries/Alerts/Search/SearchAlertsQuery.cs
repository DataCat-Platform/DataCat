namespace DataCat.Server.Application.Queries.Alerts.Search;

public sealed record SearchAlertsQuery(
    int Page,
    int PageSize,
    string? Filter)
    : IQuery<Page<SearchAlertsResponse>>, ISearchQuery, IAuthorizedQuery;
