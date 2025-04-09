namespace DataCat.Server.Infrastructure.Auth;

public sealed class DefaultIdentity : IIdentity
{
    public required string IdentityId { get; set; }
    
    public required IReadOnlyCollection<RoleClaim> RoleClaims { get; set; }
}