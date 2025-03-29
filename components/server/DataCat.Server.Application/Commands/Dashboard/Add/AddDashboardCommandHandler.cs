namespace DataCat.Server.Application.Commands.Dashboard.Add;

public sealed class AddDashboardCommandHandler(
    IRepository<DashboardEntity, Guid> dashboardRepository,
    IRepository<UserEntity, Guid> userRepository)
    : IRequestHandler<AddDashboardCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddDashboardCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId);
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Fail<Guid>(UserError.NotFound);
        }
        
        var id = Guid.NewGuid();
        var result = CreateDataSource(id, request, user);

        if (result.IsFailure)
            return Result.Fail<Guid>(result.Errors!);
            
        await dashboardRepository.AddAsync(result.Value, cancellationToken);
        return Result.Success(id);
    }
    
    private static Result<DashboardEntity> CreateDataSource(Guid id, AddDashboardCommand request, UserEntity user)
    {
        return DashboardEntity.Create(
            id,
            request.Name,
            request.Description,
            panels: [],
            user,
            sharedWith: [user],
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}