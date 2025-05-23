namespace DataCat.Auth.Keycloak.Models;

public sealed class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
    
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; init; }
}