namespace DataCat.Server.Application.Commands.Users.GetAccessTokenByCode;

public sealed record GetAccessTokenByCodeCommand(string Code) : ICommand<AccessTokenResponse>;
