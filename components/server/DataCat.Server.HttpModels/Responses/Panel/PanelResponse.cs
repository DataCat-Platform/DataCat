namespace DataCat.Server.HttpModels.Responses.Panel;

public class PanelResponse
{
    public required Guid PanelId { get; init; }
    
    public required string Title { get; init; }

    public required string Type { get; init; }

    public required PanelQueryResponse Query { get; set; }

    public required int PanelX { get; init; }
    
    public required int PanelY { get; init; }
    
    public required int Width { get; init; }
    
    public required int Height { get; init; }
    
    public required Guid DashboardId { get; init; }
}