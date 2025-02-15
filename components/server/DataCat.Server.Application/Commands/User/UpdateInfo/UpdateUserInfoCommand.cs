namespace DataCat.Server.Application.Commands.User.UpdateInfo;

public sealed record UpdateUserInfoCommand : IRequest<Result>
{
    public required string UserId { get; init; }
    
    public required string UserName { get; init; }

    public required string Email { get; init; }
}