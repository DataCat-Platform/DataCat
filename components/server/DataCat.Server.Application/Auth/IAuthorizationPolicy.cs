namespace DataCat.Server.Application.Auth;

public interface IAuthorizationPolicy
{
    bool IsAuthorized(IIdentity? user);
}