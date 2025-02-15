namespace DataCat.Server.Application.Commands.Dashboard.UpdateName;

public sealed class UpdateDashboardCommandHandler(
    IDefaultRepository<DashboardEntity, Guid> dashboardRepository)
    : IRequestHandler<UpdateDashboardCommand, Result>
{
    public async Task<Result> Handle(UpdateDashboardCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.DashboardId);
        
        var dashboard = await dashboardRepository.GetByIdAsync(id, cancellationToken);
        if (dashboard is null)
            return Result.Fail(DataSourceError.NotFound(id.ToString()));
        
        dashboard.ChangeName(request.Name);
        dashboard.ChangeDescription(request.Description);

        await dashboardRepository.UpdateAsync(dashboard, cancellationToken);
        return Result.Success();
    }
}