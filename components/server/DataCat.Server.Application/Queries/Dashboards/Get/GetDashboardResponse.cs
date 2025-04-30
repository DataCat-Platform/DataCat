namespace DataCat.Server.Application.Queries.Dashboards.Get;

public sealed record GetDashboardResponse
{
    public required Guid DashboardId { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required Guid OwnerId { get; init; }
    public required DateTime UpdatedAt { get; init; }
}

public static class GetDashboardResponseExtensions
{
    public static GetDashboardResponse ToResponse(this Dashboard dashboard)
    {
        return new GetDashboardResponse
        {
            DashboardId = dashboard.Id,
            Name = dashboard.Name,
            Description = dashboard.Description,
            OwnerId = dashboard.Owner.Id,
            UpdatedAt = dashboard.UpdatedAt
        };
    }
}