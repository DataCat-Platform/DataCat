namespace DataCat.Server.Application.Commands.Alert.Mute;

public sealed record MuteAlertCommand(
    string Id,
    TimeSpan NextExecutionTime)
    : IRequest<Result>, IAuthorizedCommand;
