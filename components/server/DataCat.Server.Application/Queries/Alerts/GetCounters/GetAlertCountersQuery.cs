namespace DataCat.Server.Application.Queries.Alerts.GetCounters;

public sealed record GetAlertCountersQuery : IQuery<List<AlertCounterResponse>>, IAuthorizedQuery; 
