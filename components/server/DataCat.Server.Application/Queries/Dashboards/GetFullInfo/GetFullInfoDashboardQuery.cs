namespace DataCat.Server.Application.Queries.Dashboards.GetFullInfo;

public sealed record GetFullInfoDashboardQuery(Guid DashboardId)
    : IQuery<GetFullInfoDashboardResponse>, IAuthorizedQuery;
