namespace DataCat.Server.Application.Commands.Alerts.Mute;

public sealed record MuteAlertCommand(
    string Id,
    TimeSpan NextExecutionTime)
    : IRequest<Result>, IAuthorizedCommand;
