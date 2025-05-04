namespace DataCat.Server.Application.Queries.Dashboards.Search;

public sealed record SearchDashboardsQuery(
    int Page, 
    int PageSize, 
    string? Filter)
    : IQuery<Page<SearchDashboardsResponse>>, ISearchQuery, IAuthorizedQuery;
