namespace DataCat.Server.Application.Queries.Dashboards.Search;

public sealed record SearchDashboardsResponse
{
    public required Guid Id { get; init; }
    public required Guid NamespaceId { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required List<string> Tags { get; init; }
    
    public static SearchDashboardsResponse ToResponse(Dashboard dashboard)
    {
        return new SearchDashboardsResponse
        {
            Id = dashboard.Id,
            NamespaceId = dashboard.NamespaceId,
            Name = dashboard.Name,
            Description = dashboard.Description,
            CreatedAt = dashboard.CreatedAt,
            UpdatedAt = dashboard.UpdatedAt,
            Tags = dashboard.Tags.Select(x => x.Value).OrderBy(x => x).ToList(),
        };
    }
}