namespace DataCat.Server.Application.Queries.Dashboards.Get;

public class GetDashboardQueryHandler(
    IRepository<Dashboard, Guid> dashboardRepository)
    : IQueryHandler<GetDashboardQuery, DashboardResponse>
{
    public async Task<Result<DashboardResponse>> Handle(GetDashboardQuery request, CancellationToken token)
    {
        var entity = await dashboardRepository.GetByIdAsync(request.DashboardId, token);
        return entity is null 
            ? Result.Fail<DashboardResponse>(DashboardError.NotFound(request.DashboardId.ToString())) 
            : Result<DashboardResponse>.Success(entity.ToResponse());
    }
}