namespace DataCat.Server.Application.Queries.Variables.GetForDashboard;

public sealed record GetVariablesForDashboardQuery(Guid DashboardId) : IQuery<List<VariableResponse>>, IAuthorizedQuery;
