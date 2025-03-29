namespace DataCat.Server.Application.Queries.Dashboard.Get;

public class GetDashboardQueryHandler(
    IRepository<DashboardEntity, Guid> dashboardRepository)
    : IRequestHandler<GetDashboardQuery, Result<GetDashboardResponse>>
{
    public async Task<Result<GetDashboardResponse>> Handle(GetDashboardQuery request, CancellationToken token)
    {
        var entity = await dashboardRepository.GetByIdAsync(request.DashboardId, token);
        return entity is null 
            ? Result.Fail<GetDashboardResponse>(DashboardError.NotFound(request.DashboardId.ToString())) 
            : Result<GetDashboardResponse>.Success(entity.ToResponse());
    }
}