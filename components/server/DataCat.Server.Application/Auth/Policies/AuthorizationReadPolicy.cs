namespace DataCat.Server.Application.Auth.Policies;

public sealed class AuthorizationReadPolicy : IAuthorizationPolicy
{
    public bool IsAuthorized(IIdentity? user, string NamespaceId)
    {
        return user is not null && user.RoleClaims.Any(x => 
            UserRole.Viewer == x.Role && x.NamespaceId == NamespaceId);
    }
}