namespace DataCat.Server.Application.Queries.Panels.Get;

public sealed record GetPanelQuery(Guid PanelId) 
    : IQuery<GetPanelResponse>, IAuthorizedQuery;
