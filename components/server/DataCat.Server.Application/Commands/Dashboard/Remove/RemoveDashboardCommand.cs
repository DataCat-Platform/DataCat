namespace DataCat.Server.Application.Commands.Dashboard.Remove;

public sealed record RemoveDashboardCommand(string DashboardId) : IRequest<Result>;
