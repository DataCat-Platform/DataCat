namespace DataCat.Server.Application.Commands.Users.Add;

[Obsolete("Outdated feature")]
public sealed record AddUserCommand : ICommand<Guid>, IAuthorizedCommand
{
    public required string UserName { get; init; }
}