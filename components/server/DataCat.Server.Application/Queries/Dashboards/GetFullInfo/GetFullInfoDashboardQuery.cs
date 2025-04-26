namespace DataCat.Server.Application.Queries.Dashboards.GetFullInfo;

public sealed record GetFullInfoDashboardQuery(Guid DashboardId)
    : IRequest<Result<GetFullInfoDashboardResponse>>, IAuthorizedQuery;
