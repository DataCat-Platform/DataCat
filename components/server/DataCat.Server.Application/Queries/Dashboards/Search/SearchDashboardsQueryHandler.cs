namespace DataCat.Server.Application.Queries.Dashboards.Search;

public class SearchDashboardsQueryHandler(
    IDashboardRepository dashboardRepository)
    : IRequestHandler<SearchDashboardsQuery, Result<Page<SearchDashboardsResponse>>>
{
    public async Task<Result<Page<SearchDashboardsResponse>>> Handle(SearchDashboardsQuery request, CancellationToken token)
    {
        var result = await dashboardRepository
            .SearchAsync(request.Filter, request.Page, request.PageSize, token);
        return Result.Success(result.ToResponsePage(SearchDashboardsResponse.ToResponse));
    }
}