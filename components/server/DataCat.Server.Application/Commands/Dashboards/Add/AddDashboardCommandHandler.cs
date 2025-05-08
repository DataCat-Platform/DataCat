namespace DataCat.Server.Application.Commands.Dashboards.Add;

public sealed class AddDashboardCommandHandler(
    IRepository<Dashboard, Guid> dashboardRepository,
    IRepository<Namespace, Guid> defaultNamespaceRepository,
    INamespaceRepository namespaceRepository)
    : ICommandHandler<AddDashboardCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddDashboardCommand request, CancellationToken cancellationToken)
    {
        var namespaceResult = await GetNamespace(request, cancellationToken);

        if (namespaceResult.IsFailure)
        {
            return Result.Fail<Guid>(namespaceResult.Errors!);
        }

        var id = Guid.NewGuid();
        var result = CreateDashboard(id, request, namespaceResult.Value);

        if (result.IsFailure)
        {
            return Result.Fail<Guid>(result.Errors!);
        }
            
        await dashboardRepository.AddAsync(result.Value, cancellationToken);
        return Result.Success(id);
    }

    private async Task<Result<Namespace>> GetNamespace(AddDashboardCommand request, CancellationToken cancellationToken)
    {
        Namespace? namespaceEntity = null;
        if (request.NamespaceId is null)
        {
            namespaceEntity = await namespaceRepository.GetDefaultNamespaceAsync(cancellationToken);
        }
        else
        {
            namespaceEntity = await defaultNamespaceRepository.GetByIdAsync(request.NamespaceId.Value, cancellationToken);
        }

        return namespaceEntity is null 
            ? Result.Fail<Namespace>(new ErrorInfo($"Cant find namespace with id: {request.NamespaceId}")) 
            : Result.Success(namespaceEntity);
    }

    private static Result<Dashboard> CreateDashboard(
        Guid id, 
        AddDashboardCommand request, 
        Namespace @namespace)
    {
        var now = DateTime.UtcNow;
        
        return Dashboard.Create(
            id,
            request.Name,
            request.Description,
            panels: [],
            @namespace.Id,
            createdAt: now,
            updatedAt: now,
            request.Tags.Select(x => new Tag(x)).ToList());
    }
}