namespace DataCat.Server.Application.Commands.Panel.Remove;

public sealed record RemovePanelCommand(string PanelId) : IRequest<Result>;
