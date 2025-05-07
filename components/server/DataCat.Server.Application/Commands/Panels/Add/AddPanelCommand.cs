namespace DataCat.Server.Application.Commands.Panels.Add;

public sealed record AddPanelCommand : ICommand<Guid>, IAuthorizedCommand
{
    public required string Title { get; init; }

    public required int Type { get; init; }

    public required string RawQuery { get; init; }
    
    public required string DataSourceId { get; init; }

    public required int PanelX { get; init; }
    
    public required int PanelY { get; init; }
    
    public required int Width { get; init; }
    
    public required int Height { get; init; }

    public required string DashboardId { get; init; }
    
    public required string StyleConfiguration { get; init; }
}