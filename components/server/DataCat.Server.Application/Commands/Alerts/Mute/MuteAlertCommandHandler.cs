namespace DataCat.Server.Application.Commands.Alerts.Mute;

public sealed class MuteAlertCommandHandler(
    IRepository<Alert, Guid> alertBaseRepository,
    IAlertRepository alertRepository)
    : ICommandHandler<MuteAlertCommand>
{
    public async Task<Result> Handle(MuteAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await alertBaseRepository.GetByIdAsync(Guid.Parse(request.Id), cancellationToken);
        if (alert is null)
            return Result.Fail(AlertError.NotFound(request.Id));

        var nextExecutionAt = DateTime.Now.Add(request.NextExecutionTime);
        alert.MuteAlert(nextExecutionAt);
        await alertRepository.UpdateAsync(alert, cancellationToken);
        return Result.Success();
    }
}