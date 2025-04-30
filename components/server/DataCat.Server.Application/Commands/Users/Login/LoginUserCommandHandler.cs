namespace DataCat.Server.Application.Commands.Users.Login;

public sealed class LoginUserCommandHandler(
    IJwtService jwtService)
    : IRequestHandler<LoginUserCommand, Result<AccessTokenResponse>>
{
    public async Task<Result<AccessTokenResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await jwtService.GetUserAccessTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        return result.IsFailure 
            ? Result.Fail<AccessTokenResponse>(UserError.InvalidCredentials) 
            : Result.Success(new AccessTokenResponse(result.Value));
    }
}