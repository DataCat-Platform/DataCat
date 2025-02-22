namespace DataCat.Server.Application.Commands.Alert.Remove;

public sealed record RemoveAlertCommand(string AlertId) : IRequest<Result>;
