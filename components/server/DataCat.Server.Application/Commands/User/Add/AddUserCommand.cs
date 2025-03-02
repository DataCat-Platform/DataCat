namespace DataCat.Server.Application.Commands.User.Add;

public sealed record AddUserCommand : IRequest<Result<Guid>>, IAuthorizedCommand
{
    public required string UserName { get; init; }

    public required string Email { get; init; }

    public required int RoleId { get; init; }
}