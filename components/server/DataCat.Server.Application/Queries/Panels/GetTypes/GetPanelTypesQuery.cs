namespace DataCat.Server.Application.Queries.Panels.GetTypes;

public sealed record GetPanelTypesQuery : IQuery<List<GetPanelTypesResponse>>, IAuthorizedQuery;
