namespace DataCat.Server.Application.Auth.Policies;

public class AuthorizationNonePolicy : IAuthorizationPolicy
{
    public bool IsAuthorized(IIdentity? user)
    {
        return true;
    }
}