namespace DataCat.Server.Application.Commands.Dashboard.AddUser;

public sealed record AddUserToDashboardCommand : IRequest<Result>, IAuthorizedCommand
{
    public required string DashboardId { get; init; }

    public required string UserId { get; init; }
}