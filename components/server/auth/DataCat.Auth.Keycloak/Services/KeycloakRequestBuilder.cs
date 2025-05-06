namespace DataCat.Auth.Keycloak.Services;

public sealed class KeycloakRequestBuilder(KeycloakOptions keycloakOptions)
{
    /// <summary>
    /// Builds the authentication request parameters for the Keycloak client.
    /// </summary>
    /// <param name="email">The email of the user requesting authentication.</param>
    /// <param name="password">The password of the user requesting authentication.</param>
    /// <returns>An array of key-value pairs representing the authentication request parameters.</returns>
    public KeyValuePair<string, string>[] BuildAuthClientRequestParameters(string email, string password)
    {
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", keycloakOptions.AuthClientId),
            new("client_secret", keycloakOptions.AuthClientSecret),
            new("scope", "openid email"),
            new("grant_type", "password"),
            new("username", email),
            new("password", password)
        };
        
        return authRequestParameters;
    }
    
    /// <summary>
    /// Builds the authentication request parameters for the Keycloak admin client using client credentials grant type.
    /// </summary>
    /// <returns>An array of key-value pairs representing the authentication request parameters.</returns>
    public KeyValuePair<string, string>[] BuildAuthServerRequestParameters()
    {
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", keycloakOptions.AdminClientId),
            new("client_secret", keycloakOptions.AdminClientSecret),
            new("scope", "openid"),
            new("grant_type", "client_credentials"),
        };
        
        return authRequestParameters;
    }
    
    /// <summary>
    /// Builds the request parameters for obtaining an access token using the authorization code flow.
    /// </summary>
    /// <param name="code">The authorization code received from the identity provider after user consent.</param>
    /// <returns>An array of key-value pairs representing the token request parameters for the authorization code grant.</returns>
    public KeyValuePair<string, string>[] BuildAuthorizationCodeRequestParameters(string code)
    {
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("grant_type", "authorization_code"),
            new("code", code),
            new("redirect_uri", keycloakOptions.RedirectUri),
            new("client_id", keycloakOptions.AuthClientId),
            new("client_secret", keycloakOptions.AuthClientSecret),
        };
        
        return authRequestParameters;
    }
}