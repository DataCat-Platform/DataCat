namespace DataCat.Server.Application.Commands.Dashboards.Remove;

public sealed class RemoveDashboardCommandHandler(
    IDashboardRepository dashboardRepository)
    : ICommandHandler<RemoveDashboardCommand>
{
    public async Task<Result> Handle(RemoveDashboardCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.DashboardId);
        await dashboardRepository.DeleteAsync(id, token);
        return Result.Success();
    }
}