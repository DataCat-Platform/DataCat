namespace DataCat.Server.Application.Commands.Dashboard.Remove;

public sealed class RemoveDashboardCommandHandler(
    IDefaultRepository<DashboardEntity, Guid> dashboardRepository)
    : IRequestHandler<RemoveDashboardCommand, Result>
{
    public async Task<Result> Handle(RemoveDashboardCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.DashboardId);
        await dashboardRepository.DeleteAsync(id, token);
        return Result.Success();
    }
}