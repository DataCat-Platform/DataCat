namespace DataCat.Server.Api.Middlewares;

public sealed class AuthorizationMiddleware(
    IIdentityProvider identityProvider,
    IHttpContextAccessor httpContextAccessor)
    : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            throw new AuthenticationException();
        }
        
        await identityProvider.LoadIdentityAsync(context.RequestAborted);
        if (identityProvider.CurrentIdentity?.IsAuthenticated != true)
        {
            throw new AuthenticationException();
        }
        
        await next(context);
    }
}

public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationMiddleware>();
    }
}