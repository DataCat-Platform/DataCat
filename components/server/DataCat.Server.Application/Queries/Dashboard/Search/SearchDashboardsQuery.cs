namespace DataCat.Server.Application.Queries.Dashboard.Search;

public sealed record SearchDashboardsQuery(
    int Page, 
    int PageSize, 
    string? Filter)
    : IRequest<Result<List<DashboardEntity>>>, ISearchQuery, IAuthorizedQuery;
