namespace DataCat.Server.Application.Commands.User.UpdateInfo;

public sealed record UpdateUserInfoCommand : IRequest<Result>, IAuthorizedCommand
{
    public required string UserId { get; init; }
    
    public required string UserName { get; init; }

    public required string Email { get; init; }
}