namespace DataCat.Server.Application.Queries.Dashboards.GetFullInfo;

public sealed record GetFullInfoDashboardResponse
{
    public required Guid DashboardId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public IEnumerable<SearchDashboardsPanelDetailsResponse>? Panels { get; init; }
    public required Guid OwnerId { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public sealed record SearchDashboardsPanelDetailsResponse
{
    public required Guid Id { get; init; }
    public string? Title { get; init; }
    public required string PanelType { get; init; }
    public required string Query { get; init; }
    public required DataCatLayoutResponse Layout { get; init; }
}

public static class GetFullInfoDashboardResponseExtensions
{
    public static GetFullInfoDashboardResponse ToFullResponse(this Dashboard dashboard)
    {
        return new GetFullInfoDashboardResponse
        {
            DashboardId = dashboard.Id,
            Name = dashboard.Name,
            Description = dashboard.Description,
            OwnerId = dashboard.Owner.Id,
            UpdatedAt = dashboard.UpdatedAt,
            Panels = dashboard?.Panels.Select(x => new SearchDashboardsPanelDetailsResponse
            {
                Id = x.Id,
                PanelType = x.Type.Name,
                Query = x.Query.RawQuery,
                Title = x.Title,
                Layout = x.DataCatLayout.ToResponse()
            }).ToList(),
        };
    }
}