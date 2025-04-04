namespace DataCat.Auth.Keycloak.Models;

public class UserKeycloakResponse
{
    [JsonPropertyName("id")]
    public required string IdentityId { get; init; }
    
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    [JsonPropertyName("email")]
    public required string Email { get; init; }
}