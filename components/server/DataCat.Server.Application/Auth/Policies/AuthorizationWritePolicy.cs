namespace DataCat.Server.Application.Auth.Policies;

public sealed class AuthorizationWritePolicy : IAuthorizationPolicy
{
    public bool IsAuthorized(IIdentity? user)
    {
        return user is not null && user.RoleClaims.Any(x => UserRole.Editor == x.Role);
    }
}