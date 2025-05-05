namespace DataCat.Server.Application.Commands.Variables.Add;

public sealed class AddVariableCommandHandler(
    IRepository<Variable, Guid> repository)
    : ICommandHandler<AddVariableCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddVariableCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();        
        var result = Variable.Create(
            id,
            request.Placeholder,
            request.Value,
            request.NamespaceId,
            request.DashboardId);

        if (result.IsFailure)
            return Result.Fail<Guid>(result.Errors!);
        
        await repository.AddAsync(result.Value, cancellationToken);

        return Result.Success(id);
    }
}