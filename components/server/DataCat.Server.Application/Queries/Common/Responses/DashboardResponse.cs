namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record DashboardResponse
{
    public required Guid Id { get; init; }
    public required Guid NamespaceId { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required List<string> Tags { get; init; }
}

public static class DashboardResponseExtensions
{
    public static DashboardResponse ToResponse(this Dashboard dashboard)
    {
        return new DashboardResponse
        {
            Id = dashboard.Id,
            NamespaceId = dashboard.NamespaceId,
            Name = dashboard.Name,
            Description = dashboard.Description,
            CreatedAt = dashboard.CreatedAt,
            UpdatedAt = dashboard.UpdatedAt,
            Tags = dashboard.Tags.Select(x => x.Value).OrderBy(x => x).ToList()
        };
    }
}