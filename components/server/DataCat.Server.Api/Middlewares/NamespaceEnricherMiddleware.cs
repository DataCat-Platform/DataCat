namespace DataCat.Server.Api.Middlewares;

public class NamespaceEnricherMiddleware(
    NamespaceContext namespaceContext,
    ILogger<RequestLoggingMiddleware> logger) : IMiddleware
{
    public const string HeaderName = "X-Namespace";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Headers.TryGetValue(HeaderName, out var headerValue))
        {
            namespaceContext.NamespaceId = headerValue.ToString();
            logger.LogDebug("Namespace header found: {Namespace}", namespaceContext.NamespaceId);
        }
        else
        {
            logger.LogDebug("Namespace header not found. Using default: {Namespace}", namespaceContext.NamespaceId);
        }

        await next(context);
    }
}

public static class NamespaceEnricherMiddlewareExtensions
{
    public static IApplicationBuilder UseNamespaceEnricher(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<NamespaceEnricherMiddleware>();
    }
}