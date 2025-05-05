namespace DataCat.Server.Application.Commands.Variables.Update;

public sealed class UpdateVariableCommandHandler(
    IVariableRepository variableRepository,
    IRepository<Variable, Guid> repository)
    : ICommandHandler<UpdateVariableCommand>
{
    public async Task<Result> Handle(UpdateVariableCommand request, CancellationToken cancellationToken)
    {
        var variable = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (variable is null)
            return Result.Fail("Variable not found");

        var newVariableResult = Variable.Create(
            variable.Id,
            request.Placeholder,
            request.Value,
            variable.NamespaceId,
            variable.DashboardId);
        
        if (newVariableResult.IsFailure)
            return Result.Fail(newVariableResult.Errors!);
        
        await variableRepository.UpdateAsync(newVariableResult.Value, cancellationToken);
        return Result.Success();
    }
}