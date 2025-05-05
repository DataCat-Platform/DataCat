namespace DataCat.Server.Application.Commands.Users.Login;

public sealed record LoginUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;
