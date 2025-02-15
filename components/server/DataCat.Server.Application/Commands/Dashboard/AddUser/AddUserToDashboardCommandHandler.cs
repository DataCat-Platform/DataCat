namespace DataCat.Server.Application.Commands.Dashboard.AddUser;

public sealed class AddUserToDashboardCommandHandler(
    IDashboardAccessRepository dashboardAccessRepository,
    IDefaultRepository<UserEntity, Guid> userRepository,
    IDefaultRepository<DashboardEntity, Guid> dashboardRepository)
    : IRequestHandler<AddUserToDashboardCommand, Result>
{
    public async Task<Result> Handle(AddUserToDashboardCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);
        if (user is null)
            return Result.Fail(UserError.NotFound);
        
        var dashboard =  await dashboardRepository.GetByIdAsync(Guid.Parse(request.DashboardId), cancellationToken);
        if (dashboard is null)
            return Result.Fail(DashboardError.NotFound(request.DashboardId));
        
        await dashboardAccessRepository.AddUserToDashboard(user, dashboard, cancellationToken);
        return Result.Success();
    }
}