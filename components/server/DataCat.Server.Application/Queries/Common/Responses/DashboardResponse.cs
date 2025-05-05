namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record DashboardResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required Guid OwnerId { get; init; }
    public required DateTime UpdatedAt { get; init; }
}

public static class DashboardResponseExtensions
{
    public static DashboardResponse ToResponse(this Dashboard dashboard)
    {
        return new DashboardResponse
        {
            Id = dashboard.Id,
            Name = dashboard.Name,
            Description = dashboard.Description,
            OwnerId = dashboard.Owner.Id,
            UpdatedAt = dashboard.UpdatedAt
        };
    }
}