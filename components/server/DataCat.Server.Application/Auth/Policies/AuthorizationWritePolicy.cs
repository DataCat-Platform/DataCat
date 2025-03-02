namespace DataCat.Server.Application.Auth.Policies;

public sealed class AuthorizationWritePolicy : IAuthorizationPolicy
{
    public bool IsAuthorized(IIdentity? user)
    {
        return user is not null && user.IsEditor;
    }
}