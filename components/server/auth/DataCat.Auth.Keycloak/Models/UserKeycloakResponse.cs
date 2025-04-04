namespace DataCat.Auth.Keycloak.Models;

public sealed record UserKeycloakResponse
{
    [JsonPropertyName("id")]
    public required string IdentityId { get; init; }
    
    [JsonPropertyName("username")]
    public required string Name { get; init; }
    
    [JsonPropertyName("email")]
    public required string Email { get; init; }
}