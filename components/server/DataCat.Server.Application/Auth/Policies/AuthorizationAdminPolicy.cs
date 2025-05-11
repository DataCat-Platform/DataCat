namespace DataCat.Server.Application.Auth.Policies;

public sealed class AuthorizationAdminPolicy : IAuthorizationPolicy
{
    public bool IsAuthorized(IIdentity? user, string NamespaceId)
    {
        return user is not null && user.RoleClaims.Any(x => 
            UserRole.Admin == x.Role && x.NamespaceId == NamespaceId);
    }
}