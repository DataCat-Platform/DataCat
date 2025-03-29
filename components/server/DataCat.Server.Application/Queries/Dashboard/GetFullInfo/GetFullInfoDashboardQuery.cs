namespace DataCat.Server.Application.Queries.Dashboard.GetFullInfo;

public sealed record GetFullInfoDashboardQuery(Guid DashboardId)
    : IRequest<Result<GetFullInfoDashboardResponse>>, IAuthorizedQuery;
