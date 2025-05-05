namespace DataCat.Server.Application.Queries.Panels.Get;

public sealed record GetPanelResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string TypeName { get; init; }
    public required RawQueryResponse Query { get; init; }
    public required DataCatLayoutResponse Layout { get; init; }
    public required Guid DashboardId { get; init; }
}

public static class GetPanelResponseExtensions
{
    public static GetPanelResponse ToResponse(this Panel panel)
    {
        return new GetPanelResponse
        {
            Id = panel.Id,
            Title = panel.Title,
            TypeName = panel.Type.Name,
            Query = panel.Query.ToResponse(),
            Layout = panel.DataCatLayout.ToResponse(),
            DashboardId = panel.DashboardId
        };
    }
}