namespace DataCat.Server.Application.Commands.Dashboards.Remove;

public sealed record RemoveDashboardCommand(string DashboardId) : ICommand, IAuthorizedCommand;
