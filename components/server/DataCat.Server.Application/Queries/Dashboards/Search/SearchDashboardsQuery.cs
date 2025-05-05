namespace DataCat.Server.Application.Queries.Dashboards.Search;

public sealed record SearchDashboardsQuery(
    int Page, 
    int PageSize, 
    SearchFilters Filters)
    : IQuery<Page<SearchDashboardsResponse>>, ISearchQuery, IAuthorizedQuery;
