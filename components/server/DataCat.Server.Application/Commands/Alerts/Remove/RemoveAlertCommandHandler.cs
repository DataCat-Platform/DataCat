namespace DataCat.Server.Application.Commands.Alerts.Remove;

public sealed class RemoveAlertCommandHandler(
    IAlertRepository alertRepository)
    : ICommandHandler<RemoveAlertCommand>
{
    public async Task<Result> Handle(RemoveAlertCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.AlertId);
        await alertRepository.DeleteAsync(id, cancellationToken);
        return Result.Success();
    }
}