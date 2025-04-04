namespace DataCat.Auth.Keycloak.Services;

public sealed class KeycloakRequestBuilder(IOptions<KeycloakOptions> keycloakOptions)
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
            new("client_id", keycloakOptions.Value.AuthClientId),
            new("client_secret", keycloakOptions.Value.AuthClientSecret),
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
            new("client_id", keycloakOptions.Value.AdminClientId),
            new("client_secret", keycloakOptions.Value.AdminClientSecret),
            new("scope", "openid"),
            new("grant_type", "client_credentials"),
        };
        
        return authRequestParameters;
    }
}