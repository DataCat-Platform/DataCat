namespace DataCat.Server.Application.Commands.Users.GetAccessTokenByCode;

public sealed class GetAccessTokenByCodeCommandHandler(
    IJwtService jwtService)
    : ICommandHandler<GetAccessTokenByCodeCommand, AccessTokenResponse>
{
    public async Task<Result<AccessTokenResponse>> Handle(GetAccessTokenByCodeCommand request, CancellationToken cancellationToken)
    {
        var accessTokenResult = await jwtService.GetAccessTokenByAuthorizationCodeAsync(request.Code, cancellationToken);
        
        return accessTokenResult.IsFailure 
            ? Result.Fail<AccessTokenResponse>(accessTokenResult.Errors!) 
            : Result.Success(new AccessTokenResponse(accessTokenResult.Value));
    }
}