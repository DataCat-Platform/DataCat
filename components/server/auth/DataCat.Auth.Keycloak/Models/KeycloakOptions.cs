namespace DataCat.Auth.Keycloak.Models;

public sealed record KeycloakOptions
{
    public string BaseUrl { get; init; } = string.Empty;
    
    public string AdminUrl { get; init; } = string.Empty;

    public string TokenUrl { get; init; } = string.Empty;
    
    public string AuthUrl { get; init; } = string.Empty;
    
    public string RedirectUri  { get; init; } = string.Empty;

    public string AdminClientId { get; init; } = string.Empty;

    public string AdminClientSecret { get; init; } = string.Empty;

    public string AuthClientId { get; init; } = string.Empty;

    public string AuthClientSecret { get; init; } = string.Empty;
}