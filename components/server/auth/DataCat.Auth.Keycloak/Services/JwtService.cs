namespace DataCat.Auth.Keycloak.Services;

public sealed class JwtService(
    HttpClient httpClient,
    KeycloakRequestBuilder keycloakRequestBuilder) : IJwtService
{
    private static readonly ErrorInfo AuthenticationFailed = new(
        "Keycloak.AuthenticationFailed",
        "Failed to acquire access token do to authentication failure");

    public async Task<Result<string>> GetUserAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = keycloakRequestBuilder.BuildAuthClientRequestParameters(email, password);
            using var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            var response = await httpClient.PostAsync(string.Empty, authorizationRequestContent, cancellationToken);

            response.EnsureSuccessStatusCode();

            var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken: cancellationToken);

            return authorizationToken is null 
                ? Result.Fail<string>(AuthenticationFailed) 
                : Result.Success(authorizationToken.AccessToken);
        }
        catch (HttpRequestException)
        {
            return Result.Fail<string>(AuthenticationFailed);
        }
    }
    
    public async Task<Result<string>> GetServerAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = keycloakRequestBuilder.BuildAuthServerRequestParameters();
            using var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            var response = await httpClient.PostAsync(string.Empty, authorizationRequestContent, cancellationToken);

            response.EnsureSuccessStatusCode();

            var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken: cancellationToken);

            return authorizationToken is null 
                ? Result.Fail<string>(AuthenticationFailed) 
                : Result.Success(authorizationToken.AccessToken);
        }
        catch (HttpRequestException)
        {
            return Result.Fail<string>(AuthenticationFailed);
        }
    }
}