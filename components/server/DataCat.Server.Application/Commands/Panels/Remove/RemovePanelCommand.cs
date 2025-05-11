namespace DataCat.Server.Application.Commands.Panels.Remove;

public sealed record RemovePanelCommand(string PanelId) : ICommand, IAuthorizedCommand;
