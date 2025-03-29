namespace DataCat.Server.Application.Queries.Dashboard.Search;

public sealed record SearchDashboardsResponse
{
    public required Guid DashboardId { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required Guid OwnerId { get; init; }
    public required DateTime UpdatedAt { get; init; }
    
    public static SearchDashboardsResponse ToResponse(DashboardEntity dashboard)
    {
        return new SearchDashboardsResponse
        {
            DashboardId = dashboard.Id,
            Name = dashboard.Name,
            Description = dashboard.Description,
            OwnerId = dashboard.Owner.Id,
            UpdatedAt = dashboard.UpdatedAt
        };
    }
}