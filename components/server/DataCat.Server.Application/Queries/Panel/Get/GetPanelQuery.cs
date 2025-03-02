namespace DataCat.Server.Application.Queries.Panel.Get;

public sealed record GetPanelQuery(Guid PanelId) : IRequest<Result<PanelEntity>>, IAuthorizedQuery;
