namespace DataCat.Server.Application.Commands.Users.Remove;

[Obsolete("Outdated feature")]
public sealed record RemoveUserCommand(string UserId) : IRequest<Result>, IAuthorizedCommand;
