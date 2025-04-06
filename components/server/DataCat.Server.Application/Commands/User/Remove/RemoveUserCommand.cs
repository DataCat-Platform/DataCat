namespace DataCat.Server.Application.Commands.User.Remove;

[Obsolete("Outdated feature")]
public sealed record RemoveUserCommand(string UserId) : IRequest<Result>, IAuthorizedCommand;
