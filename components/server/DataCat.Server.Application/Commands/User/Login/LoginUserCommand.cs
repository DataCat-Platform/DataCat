namespace DataCat.Server.Application.Commands.User.Login;

public sealed record LoginUserCommand(string Email, string Password)
    : IRequest<Result<AccessTokenResponse>>;
