namespace DataCat.Server.Application.Commands.Dashboard.AddUser;

public sealed class AddUserToDashboardCommandHandler(
    IDashboardRepository dashboardRepository,
    IRepository<UserEntity, Guid> userRepository,
    IRepository<DashboardEntity, Guid> dashboardBaseRepository)
    : IRequestHandler<AddUserToDashboardCommand, Result>
{
    public async Task<Result> Handle(AddUserToDashboardCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);
        if (user is null)
            return Result.Fail(UserError.NotFound);
        
        var dashboard =  await dashboardBaseRepository.GetByIdAsync(Guid.Parse(request.DashboardId), cancellationToken);
        if (dashboard is null)
            return Result.Fail(DashboardError.NotFound(request.DashboardId));
        
        await dashboardRepository.AddUserToDashboard(user, dashboard, cancellationToken);
        return Result.Success();
    }
}