namespace DataCat.Server.Application.Commands.Variables.Remove;

public sealed class RemoveVariableCommandHandler(
    IVariableRepository variableRepository) : ICommandHandler<RemoveVariableCommand>
{
    public async Task<Result> Handle(RemoveVariableCommand request, CancellationToken cancellationToken)
    {
        await variableRepository.DeleteAsync(request.Id, cancellationToken);
        return Result.Success();
    }
}
