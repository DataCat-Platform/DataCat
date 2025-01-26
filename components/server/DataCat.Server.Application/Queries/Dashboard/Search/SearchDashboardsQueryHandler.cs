namespace DataCat.Server.Application.Queries.Dashboard.Search;

public class SearchDashboardsQueryHandler(
    IDefaultRepository<DashboardEntity, Guid> dashboardRepository)
    : IRequestHandler<SearchDashboardsQuery, Result<IEnumerable<DashboardEntity>>>
{
    public async Task<Result<IEnumerable<DashboardEntity>>> Handle(SearchDashboardsQuery request, CancellationToken token)
    {
        var result = await dashboardRepository.SearchAsync(request.Filter, request.Page, request.PageSize, token);
        return Result.Success(result);
    }
}