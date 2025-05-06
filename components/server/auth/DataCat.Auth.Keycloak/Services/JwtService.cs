namespace DataCat.Auth.Keycloak.Services;

public sealed class JwtService(
    HttpClient httpClient,
    KeycloakRequestBuilder keycloakRequestBuilder,
    IMemoryCache cache,
    KeycloakMetricsContainer keycloakMetricsContainer) : IJwtService
{
    private static readonly ErrorInfo AuthenticationFailed = new(
        "Keycloak.AuthenticationFailed",
        "Failed to acquire access token do to authentication failure");
    
    private const string AdminAccessTokenKey = "AdminAccessToken";
    
    private const string UserAuthType = "user";
    private const string ServerAuthType = "server";
    private const string AuthorizationCodeAuthType = "authorization_code";
    private const string TokenKeycloakEndpoint = "token";

    public async Task<Result<string>> GetUserAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        keycloakMetricsContainer.AddAuthenticationAttempt(UserAuthType);
        keycloakMetricsContainer.AddTokenRequest(UserAuthType);
        
        try
        {
            var authRequestParameters = keycloakRequestBuilder.BuildAuthClientRequestParameters(email, password);
            using var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            keycloakMetricsContainer.AddKeycloakApiCall(TokenKeycloakEndpoint);
            var response = await httpClient.PostAsync(string.Empty, authorizationRequestContent, cancellationToken);

            response.EnsureSuccessStatusCode();

            var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken: cancellationToken);
            
            if (authorizationToken is null)
            {
                keycloakMetricsContainer.AddAuthenticationFailure(UserAuthType);
                keycloakMetricsContainer.AddKeycloakApiFailure(TokenKeycloakEndpoint);
                return Result.Fail<string>(AuthenticationFailed);
            }

            var duration = stopwatch.ElapsedMilliseconds;
            keycloakMetricsContainer.RecordAuthenticationDuration(UserAuthType, duration, isSuccess: true);
            keycloakMetricsContainer.RecordTokenRequestDuration(UserAuthType, duration, isSuccess: true);
            keycloakMetricsContainer.RecordKeycloakApiDuration(TokenKeycloakEndpoint, duration, isSuccess: true);
            
            return Result.Success(authorizationToken.AccessToken);
        }
        catch (HttpRequestException)
        {
            var duration = stopwatch.ElapsedMilliseconds;
            
            keycloakMetricsContainer.AddAuthenticationFailure(UserAuthType);
            keycloakMetricsContainer.AddKeycloakApiFailure(TokenKeycloakEndpoint);
            keycloakMetricsContainer.RecordAuthenticationDuration(UserAuthType, duration, isSuccess: false);
            keycloakMetricsContainer.RecordTokenRequestDuration(TokenKeycloakEndpoint, duration, isSuccess: false);
            keycloakMetricsContainer.RecordKeycloakApiDuration(TokenKeycloakEndpoint, duration, isSuccess: false);
            
            return Result.Fail<string>(AuthenticationFailed);
        }
    }
    
    public async Task<Result<string>> GetServerAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        
        if (cache.TryGetValue(AdminAccessTokenKey, out AuthorizationToken? token))
        {
            keycloakMetricsContainer.AddTokenCacheHit(ServerAuthType);
            return Result.Success(token!.AccessToken);
        }
        
        keycloakMetricsContainer.AddAuthenticationAttempt(ServerAuthType);
        keycloakMetricsContainer.AddTokenRequest(ServerAuthType);
        
        try
        {
            var authRequestParameters = keycloakRequestBuilder.BuildAuthServerRequestParameters();
            using var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);
            
            keycloakMetricsContainer.AddKeycloakApiCall(TokenKeycloakEndpoint);
            var response = await httpClient.PostAsync(string.Empty, authorizationRequestContent, cancellationToken);

            response.EnsureSuccessStatusCode();

            var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken: cancellationToken);

            if (authorizationToken is null)
            {
                keycloakMetricsContainer.AddAuthenticationFailure(ServerAuthType);
                keycloakMetricsContainer.AddKeycloakApiFailure(TokenKeycloakEndpoint);
                return Result.Fail<string>(AuthenticationFailed);
            }
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(authorizationToken!.ExpiresIn))
                .SetPriority(CacheItemPriority.High);

            cache.Set(AdminAccessTokenKey, authorizationToken, cacheOptions);
            
            var duration = stopwatch.ElapsedMilliseconds;
            keycloakMetricsContainer.RecordAuthenticationDuration(ServerAuthType, duration, isSuccess: true);
            keycloakMetricsContainer.RecordTokenRequestDuration(ServerAuthType, duration, isSuccess: true);
            keycloakMetricsContainer.RecordKeycloakApiDuration(TokenKeycloakEndpoint, duration, isSuccess: true);
            
            return Result.Success(authorizationToken.AccessToken);
        }
        catch (HttpRequestException)
        {
            var duration = stopwatch.ElapsedMilliseconds;
            
            keycloakMetricsContainer.AddAuthenticationFailure(ServerAuthType);
            keycloakMetricsContainer.AddKeycloakApiFailure(TokenKeycloakEndpoint);
            keycloakMetricsContainer.RecordAuthenticationDuration(ServerAuthType, duration, isSuccess: false);
            keycloakMetricsContainer.RecordTokenRequestDuration(ServerAuthType, duration, isSuccess: false);
            keycloakMetricsContainer.RecordKeycloakApiDuration(TokenKeycloakEndpoint, duration, isSuccess: false);
            
            return Result.Fail<string>(AuthenticationFailed);
        }
    }

    public async Task<Result<string>> GetAccessTokenByAuthorizationCodeAsync(
    string code,
    CancellationToken cancellationToken = default)
{
    var stopwatch = Stopwatch.StartNew();
    keycloakMetricsContainer.AddAuthenticationAttempt(AuthorizationCodeAuthType);
    keycloakMetricsContainer.AddTokenRequest(AuthorizationCodeAuthType);

    try
    {
        var requestParameters = keycloakRequestBuilder.BuildAuthorizationCodeRequestParameters(code);
        using var authorizationRequestContent = new FormUrlEncodedContent(requestParameters);

        keycloakMetricsContainer.AddKeycloakApiCall(TokenKeycloakEndpoint);
        var response = await httpClient.PostAsync(string.Empty, authorizationRequestContent, cancellationToken);

        response.EnsureSuccessStatusCode();

        var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken: cancellationToken);

        if (authorizationToken is null)
        {
            keycloakMetricsContainer.AddAuthenticationFailure(AuthorizationCodeAuthType);
            keycloakMetricsContainer.AddKeycloakApiFailure(TokenKeycloakEndpoint);
            return Result.Fail<string>(AuthenticationFailed);
        }

        var duration = stopwatch.ElapsedMilliseconds;
        keycloakMetricsContainer.RecordAuthenticationDuration(AuthorizationCodeAuthType, duration, isSuccess: true);
        keycloakMetricsContainer.RecordTokenRequestDuration(AuthorizationCodeAuthType, duration, isSuccess: true);
        keycloakMetricsContainer.RecordKeycloakApiDuration(TokenKeycloakEndpoint, duration, isSuccess: true);

        return Result.Success(authorizationToken.AccessToken);
    }
    catch (HttpRequestException)
    {
        var duration = stopwatch.ElapsedMilliseconds;

        keycloakMetricsContainer.AddAuthenticationFailure(AuthorizationCodeAuthType);
        keycloakMetricsContainer.AddKeycloakApiFailure(TokenKeycloakEndpoint);
        keycloakMetricsContainer.RecordAuthenticationDuration(AuthorizationCodeAuthType, duration, isSuccess: false);
        keycloakMetricsContainer.RecordTokenRequestDuration(AuthorizationCodeAuthType, duration, isSuccess: false);
        keycloakMetricsContainer.RecordKeycloakApiDuration(TokenKeycloakEndpoint, duration, isSuccess: false);

        return Result.Fail<string>(AuthenticationFailed);
    }
}
}