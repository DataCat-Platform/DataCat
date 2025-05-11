namespace DataCat.Server.Application.Behaviors;

public interface IAuthorizedRequest
{
    string GetName() => "None";
    IAuthorizationPolicy GetPolicy() => new AuthorizationNonePolicy();
}

public interface IAuthorizedCommand : IAuthorizedRequest
{
    string IAuthorizedRequest.GetName() => "Write";
    IAuthorizationPolicy IAuthorizedRequest.GetPolicy() => new AuthorizationWritePolicy();
}

public interface IAuthorizedQuery : IAuthorizedRequest
{
    string IAuthorizedRequest.GetName() => "Read";
    IAuthorizationPolicy IAuthorizedRequest.GetPolicy() => new AuthorizationReadPolicy();
}

public interface IAdminRequest : IAuthorizedRequest
{
    string IAuthorizedRequest.GetName() => "Admin";
    IAuthorizationPolicy IAuthorizedRequest.GetPolicy() => new AuthorizationAdminPolicy();
}

public sealed class AuthorizationBehavior<TRequest, TResponse>(IIdentity identity, NamespaceContext namespaceContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IAuthorizedRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
    {
        var policy = request.GetPolicy();
        
        if (!policy.IsAuthorized(identity, namespaceContext.NamespaceId))
        {
            throw new AuthenticationException(
                $"Access denied: You are not authorized to perform '{request.GetName()}' operations in namespace '{namespaceContext.NamespaceId}'.");
        }
        
        var response = await next();
        return response;
    }
}