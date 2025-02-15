namespace DataCat.Server.Application.Queries.Dashboard.Get;

public class GetDashboardQueryHandler(
    IDefaultRepository<DashboardEntity, Guid> dashboardRepository)
    : IRequestHandler<GetDashboardQuery, Result<DashboardEntity>>
{
    public async Task<Result<DashboardEntity>> Handle(GetDashboardQuery request, CancellationToken token)
    {
        var entity = await dashboardRepository.GetByIdAsync(request.DashboardId, token);
        return entity is null 
            ? Result.Fail<DashboardEntity>(DashboardError.NotFound(request.DashboardId.ToString())) 
            : Result<DashboardEntity>.Success(entity);
    }
}