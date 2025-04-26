namespace DataCat.Server.Application.Queries.Dashboards.Get;

public sealed record GetDashboardQuery(Guid DashboardId)
    : IRequest<Result<GetDashboardResponse>>, IAuthorizedQuery;
