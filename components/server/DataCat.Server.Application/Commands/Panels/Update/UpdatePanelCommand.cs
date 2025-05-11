namespace DataCat.Server.Application.Commands.Panels.Update;

public sealed record UpdatePanelCommand : ICommand, IAuthorizedCommand
{
    public required string PanelId { get; init; }
    
    public required string Title { get; init; }

    public required int Type { get; init; }

    public required string RawQuery { get; init; }
    
    public required string DataSourceId { get; init; }

    public required string Layout { get; init; }
    
    public required string StyleConfiguration { get; init; }
}