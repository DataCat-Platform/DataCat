namespace DataCat.Server.Application.Commands.Dashboards.UpdateName;

public sealed class UpdateDashboardCommandHandler(
    IRepository<Dashboard, Guid> dashboardBaseRepository,
    IDashboardRepository dashboardRepository)
    : IRequestHandler<UpdateDashboardCommand, Result>
{
    public async Task<Result> Handle(UpdateDashboardCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.DashboardId);
        
        var dashboard = await dashboardBaseRepository.GetByIdAsync(id, cancellationToken);
        if (dashboard is null)
            return Result.Fail(DataSourceError.NotFound(id.ToString()));
        
        dashboard.ChangeName(request.Name);
        dashboard.ChangeDescription(request.Description);

        await dashboardRepository.UpdateAsync(dashboard, cancellationToken);
        return Result.Success();
    }
}