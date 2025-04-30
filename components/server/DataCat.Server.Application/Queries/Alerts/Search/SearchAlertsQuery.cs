namespace DataCat.Server.Application.Queries.Alerts.Search;

public sealed record SearchAlertsQuery(
    int Page,
    int PageSize,
    string? Filter)
    : IRequest<Result<Page<SearchAlertsResponse>>>, ISearchQuery, IAuthorizedQuery;
