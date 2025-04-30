namespace DataCat.Server.Application.Queries.Panels.Get;

public sealed record GetPanelQuery(Guid PanelId) 
    : IRequest<Result<GetPanelResponse>>, IAuthorizedQuery;
