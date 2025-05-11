namespace DataCat.Server.Application.Queries.Alerts.Search;

public sealed record SearchAlertsQuery(
    int Page,
    int PageSize,
    SearchFilters Filters)
    : IQuery<Page<SearchAlertsResponse>>, ISearchQuery, IAuthorizedQuery;
