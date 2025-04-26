namespace DataCat.Server.Application.Commands.Panels.Remove;

public sealed record RemovePanelCommand(string PanelId) : IRequest<Result>, IAuthorizedCommand;
