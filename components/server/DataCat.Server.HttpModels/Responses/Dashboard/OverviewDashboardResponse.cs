namespace DataCat.Server.HttpModels.Responses.Dashboard;

public class OverviewDashboardResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public IEnumerable<PanelDetailsDto>? Panels { get; init; }
    public required Guid OwnerId { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public class PanelDetailsDto
{
    public required Guid Id { get; init; }
    public string? Title { get; init; }
    public required string PanelType { get; init; }
    public required string QueryId { get; init; }
    public required DataCatLayoutDto Layout { get; init; }
}

public class DataCatLayoutDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}