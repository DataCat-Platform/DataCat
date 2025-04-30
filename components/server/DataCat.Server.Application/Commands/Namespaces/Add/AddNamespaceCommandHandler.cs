namespace DataCat.Server.Application.Commands.Namespaces.Add;

public sealed class AddNamespaceCommandHandler(
    IRepository<Namespace, Guid> repository,
    INamespaceRepository namespaceRepository)
    : IRequestHandler<AddNamespaceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddNamespaceCommand request, CancellationToken cancellationToken)
    {
        var @namespace = await namespaceRepository.GetByNameAsync(request.Name, cancellationToken);

        if (@namespace is not null)
        {
            return Result.Fail<Guid>(new ErrorInfo($"Namespace with name {request.Name} is already exists"));
        }
        
        var id = Guid.NewGuid();
        var namespaceEntityResult = Namespace.Create(id, request.Name, []);
        if (namespaceEntityResult.IsFailure)
        {
            return Result.Fail<Guid>(namespaceEntityResult.Errors!);
        }
        
        await repository.AddAsync(namespaceEntityResult.Value, cancellationToken);

        return Result.Success(id);
    }
}