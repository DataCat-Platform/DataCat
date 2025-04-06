namespace DataCat.Server.Application.Commands.Dashboard.Add;

public sealed class AddDashboardCommandHandler(
    IRepository<DashboardEntity, Guid> dashboardRepository,
    IRepository<UserEntity, Guid> userRepository,
    IRepository<NamespaceEntity, Guid> defaultNamespaceRepository,
    INamespaceRepository namespaceRepository)
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
        
        var namespaceResult = await GetNamespace(request, cancellationToken);

        if (namespaceResult.IsFailure)
        {
            return Result.Fail<Guid>(namespaceResult.Errors!);
        }

        var id = Guid.NewGuid();
        var result = CreateDashboard(id, request, user, namespaceResult.Value);

        if (result.IsFailure)
        {
            return Result.Fail<Guid>(result.Errors!);
        }
            
        await dashboardRepository.AddAsync(result.Value, cancellationToken);
        return Result.Success(id);
    }

    private async Task<Result<NamespaceEntity>> GetNamespace(AddDashboardCommand request, CancellationToken cancellationToken)
    {
        NamespaceEntity? namespaceEntity = null;
        if (request.NamespaceId is null)
        {
            namespaceEntity = await namespaceRepository.GetDefaultNamespaceAsync(cancellationToken);
        }
        else
        {
            namespaceEntity = await defaultNamespaceRepository.GetByIdAsync(request.NamespaceId.Value, cancellationToken);
        }

        return namespaceEntity is null 
            ? Result.Fail<NamespaceEntity>(new ErrorInfo($"Cant find namespace with id: {request.NamespaceId}")) 
            : Result.Success(namespaceEntity);
    }

    private static Result<DashboardEntity> CreateDashboard(
        Guid id, 
        AddDashboardCommand request, 
        UserEntity user, 
        NamespaceEntity namespaceEntity)
    {
        return DashboardEntity.Create(
            id,
            request.Name,
            request.Description,
            panels: [],
            user,
            sharedWith: [user],
            namespaceEntity.Id,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}