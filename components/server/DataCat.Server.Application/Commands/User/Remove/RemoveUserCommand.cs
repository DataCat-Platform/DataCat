namespace DataCat.Server.Application.Commands.User.Remove;

public sealed record RemoveUserCommand(string UserId) : IRequest<Result>, IAuthorizedCommand;
