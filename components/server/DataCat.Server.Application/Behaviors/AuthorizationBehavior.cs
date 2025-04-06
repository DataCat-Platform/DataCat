namespace DataCat.Server.Application.Behaviors;

public interface IAuthorizedRequest
{
    IAuthorizationPolicy GetPolicy() => new AuthorizationNonePolicy();
}

public interface IAuthorizedCommand : IAuthorizedRequest
{
    IAuthorizationPolicy IAuthorizedRequest.GetPolicy() => new AuthorizationWritePolicy();
}

public interface IAuthorizedQuery : IAuthorizedRequest
{
    IAuthorizationPolicy IAuthorizedRequest.GetPolicy() => new AuthorizationReadPolicy();
}

public interface IAdminRequest : IAuthorizedRequest
{
    IAuthorizationPolicy IAuthorizedRequest.GetPolicy() => new AuthorizationAdminPolicy();
}

public sealed class AuthorizationBehavior<TRequest, TResponse>(IIdentity identity)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IAuthorizedRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
    {
        var policy = request.GetPolicy();
        if (!policy.IsAuthorized(identity))
        {
            throw new AuthenticationException("You are not authorized to perform this operation.");
        }
        var response = await next();
        return response;
    }
}