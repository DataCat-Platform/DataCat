namespace DataCat.Server.Application.Queries.Dashboards.GetFullInfo;

public sealed record GetFullInfoDashboardResponse
{
    public required Guid Id { get; init; }
    public required Guid NamespaceId { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required IEnumerable<SearchDashboardsPanelDetailsResponse>? Panels { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required List<string> Tags { get; init; }
}

public sealed record SearchDashboardsPanelDetailsResponse
{
    public required Guid Id { get; init; }
    public string? Title { get; init; }
    public required string PanelType { get; init; }
    public required string Query { get; init; }
    public required string Layout { get; init; }
}

public static class GetFullInfoDashboardResponseExtensions
{
    public static GetFullInfoDashboardResponse ToFullResponse(this Dashboard dashboard)
    {
        return new GetFullInfoDashboardResponse
        {
            Id = dashboard.Id,
            NamespaceId = dashboard.NamespaceId,
            Name = dashboard.Name,
            Description = dashboard.Description,
            CreatedAt = dashboard.CreatedAt,
            UpdatedAt = dashboard.UpdatedAt,
            Tags = dashboard.Tags.Select(x => x.Value).OrderBy(x => x).ToList(),
            Panels = dashboard?.Panels.Select(x => new SearchDashboardsPanelDetailsResponse
            {
                Id = x.Id,
                PanelType = x.Type.Name,
                Query = x.Query.RawQuery,
                Title = x.Title,
                Layout = x.DataCatLayout.Settings
            }).ToList(),
        };
    }
}