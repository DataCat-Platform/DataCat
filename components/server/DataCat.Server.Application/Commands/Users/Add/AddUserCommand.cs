namespace DataCat.Server.Application.Commands.Users.Add;

[Obsolete("Outdated feature")]
public sealed record AddUserCommand : IRequest<Result<Guid>>, IAuthorizedCommand
{
    public required string UserName { get; init; }
}