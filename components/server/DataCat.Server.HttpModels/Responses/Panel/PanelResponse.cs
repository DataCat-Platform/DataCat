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

public class PanelQueryResponse
{
    public required string Query { get; init; }

    public required string DataSourceName { get; init; }

    public required string DataSourceType { get; init; }

    public required string ConnectionString { get; init; }
}