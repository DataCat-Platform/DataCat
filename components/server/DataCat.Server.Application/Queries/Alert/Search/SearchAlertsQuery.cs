namespace DataCat.Server.Application.Queries.Alert.Search;

public sealed record SearchAlertsQuery(
    int Page,
    int PageSize,
    string? Filter)
    : IRequest<Result<Page<SearchAlertsResponse>>>, ISearchQuery, IAuthorizedQuery;
