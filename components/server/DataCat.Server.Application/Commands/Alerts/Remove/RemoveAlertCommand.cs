namespace DataCat.Server.Application.Commands.Alerts.Remove;

public sealed record RemoveAlertCommand(string AlertId) : ICommand, IAuthorizedCommand;
