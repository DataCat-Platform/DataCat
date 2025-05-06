namespace DataCat.Auth.Keycloak.Services;

public sealed class KeycloakOidcRedirectService(KeycloakOptions options) : IOidcRedirectService
{
    public string GenerateRedirectUrl()
    {
        var redirectUri = options.RedirectUri; 
        var clientId = options.AuthClientId;

        var authUrl = $"{options.AuthUrl}" +
            $"?client_id={clientId}" +
            $"&response_type=code" +
            $"&scope=openid" +
            $"&redirect_uri={Uri.EscapeDataString(redirectUri)}";
        
        return authUrl;
    }
}