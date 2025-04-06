namespace DataCat.Auth.Keycloak.Models;

public sealed record RealmMappingsResponse
{
    [JsonPropertyName("realmMappings")]
    public required List<UserRoleMappingsResponse> RealmMappings { get; init; }
}

public sealed record UserRoleMappingsResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }
    
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    [JsonPropertyName("description")]
    public required string Description { get; init; }
}