namespace DataCat.Server.Application.Commands.Alert.Remove;

public sealed class RemoveAlertCommandHandler(
    IDefaultRepository<AlertEntity, Guid> alertRepository)
    : IRequestHandler<RemoveAlertCommand, Result>
{
    public async Task<Result> Handle(RemoveAlertCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.AlertId);
        await alertRepository.DeleteAsync(id, cancellationToken);
        return Result.Success();
    }
}