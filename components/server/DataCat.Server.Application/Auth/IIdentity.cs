namespace DataCat.Server.Application.Auth;

public interface IIdentity
{
    string IdentityId { get; set; }
    
    IReadOnlyCollection<RoleClaim> RoleClaims { get; set; }
}