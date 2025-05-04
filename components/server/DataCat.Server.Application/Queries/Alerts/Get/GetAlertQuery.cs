namespace DataCat.Server.Application.Queries.Alerts.Get;

public sealed record GetAlertQuery(Guid AlertId) : IQuery<GetAlertResponse>, IAuthorizedQuery;
