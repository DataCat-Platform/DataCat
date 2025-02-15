namespace DataCat.Server.Application.Commands.User.UpdateRole;

public sealed record UpdateUserRoleCommand : IRequest<Result>
{
    public required string UserId { get; init; }
    
    public required int RoleId { get; init; }
}