namespace DataCat.Server.Application.Commands.Dashboard.AddUser;

public sealed record AddUserToDashboardCommand : IRequest<Result>
{
    public required string DashboardId { get; set; }

    public required string UserId { get; set; }
}