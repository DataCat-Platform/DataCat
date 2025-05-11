namespace DataCat.Server.Application.Auth.Policies;

public class AuthorizationNonePolicy : IAuthorizationPolicy
{
    public bool IsAuthorized(IIdentity? user, string NamespaceId)
    {
        return true;
    }
}