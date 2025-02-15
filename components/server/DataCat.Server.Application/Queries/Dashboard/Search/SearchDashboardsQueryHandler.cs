namespace DataCat.Server.Application.Queries.Dashboard.Search;

public class SearchDashboardsQueryHandler(
    IDefaultRepository<DashboardEntity, Guid> dashboardRepository)
    : IRequestHandler<SearchDashboardsQuery, Result<List<DashboardEntity>>>
{
    public async Task<Result<List<DashboardEntity>>> Handle(SearchDashboardsQuery request, CancellationToken token)
    {
        var result = await dashboardRepository
            .SearchAsync(request.Filter, request.Page, request.PageSize, token)
            .ToListAsync(cancellationToken: token);
        return Result.Success(result);
    }
}