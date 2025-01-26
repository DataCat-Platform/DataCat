namespace DataCat.Server.HttpModels.Responses.Dashboard;

public class HomeSearchDashboardResponse
{
    public required Guid DashboardId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Guid OwnerId { get; init; }
    public required DateTime UpdatedAt { get; init; }
}