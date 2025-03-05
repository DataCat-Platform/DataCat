namespace DataCat.Server.Application.Commands.Alert.Mute;

public sealed class MuteAlertCommandHandler(
    IDefaultRepository<AlertEntity, Guid> alertRepository)
    : IRequestHandler<MuteAlertCommand, Result>
{
    public async Task<Result> Handle(MuteAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await alertRepository.GetByIdAsync(Guid.Parse(request.Id), cancellationToken);
        if (alert is null)
            return Result.Fail(AlertError.NotFound(request.Id));

        var nextExecutionAt = DateTime.Now.Add(request.NextExecutionTime);
        alert.MuteAlert(nextExecutionAt);
        await alertRepository.UpdateAsync(alert, cancellationToken);
        return Result.Success();
    }
}