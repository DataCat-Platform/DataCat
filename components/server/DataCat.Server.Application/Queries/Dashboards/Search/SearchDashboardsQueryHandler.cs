namespace DataCat.Server.Application.Queries.Dashboards.Search;

public class SearchDashboardsQueryHandler(
    IDashboardRepository dashboardRepository)
    : IQueryHandler<SearchDashboardsQuery, Page<SearchDashboardsResponse>>
{
    public async Task<Result<Page<SearchDashboardsResponse>>> Handle(SearchDashboardsQuery request, CancellationToken token)
    {
        var result = await dashboardRepository
            .SearchAsync(request.Filters, request.Page, request.PageSize, token);
        return Result.Success(result.ToResponsePage(SearchDashboardsResponse.ToResponse));
    }
}