namespace DataCat.Server.Application.Queries.Panel.Get;

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
    public static GetPanelResponse ToResponse(this PanelEntity panelEntity)
    {
        return new GetPanelResponse
        {
            Id = panelEntity.Id,
            Title = panelEntity.Title,
            TypeName = panelEntity.Type.Name,
            Query = panelEntity.QueryEntity.ToResponse(),
            Layout = panelEntity.DataCatLayout.ToResponse(),
            DashboardId = panelEntity.DashboardId
        };
    }
}