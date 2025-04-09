namespace DataCat.Auth.Keycloak.Models;

public class KeycloakIdentity : IIdentity
{
    public required string IdentityId { get; set; }
    public required IReadOnlyCollection<RoleClaim> RoleClaims { get; set; }
}