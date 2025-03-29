namespace DataCat.Server.Application.Queries.Alert.Get;

public sealed record GetAlertQuery(Guid AlertId) : IRequest<Result<GetAlertResponse>>, IAuthorizedQuery;
