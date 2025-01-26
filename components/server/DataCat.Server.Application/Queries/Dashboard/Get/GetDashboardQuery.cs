namespace DataCat.Server.Application.Queries.Dashboard.Get;

public sealed record GetDashboardQuery(Guid DashboardId) : IRequest<Result<DashboardEntity>>;
