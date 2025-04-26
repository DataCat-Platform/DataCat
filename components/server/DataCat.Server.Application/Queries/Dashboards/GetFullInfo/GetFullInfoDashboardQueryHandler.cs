namespace DataCat.Server.Application.Queries.Dashboards.GetFullInfo;

public sealed class GetFullInfoDashboardQueryHandler(
    IRepository<Dashboard, Guid> dashboardRepository)
    : IRequestHandler<GetFullInfoDashboardQuery, Result<GetFullInfoDashboardResponse>>
{
    public async Task<Result<GetFullInfoDashboardResponse>> Handle(GetFullInfoDashboardQuery request, CancellationToken token)
    {
        var entity = await dashboardRepository.GetByIdAsync(request.DashboardId, token);
        return entity is null 
            ? Result.Fail<GetFullInfoDashboardResponse>(DashboardError.NotFound(request.DashboardId.ToString())) 
            : Result<GetFullInfoDashboardResponse>.Success(entity.ToFullResponse());
    }
}