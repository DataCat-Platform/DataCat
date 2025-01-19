namespace DataCat.Server.Api.Middlewares;

public class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger) : IMiddleware
{
    private readonly List<string> ByPassRequestLogging = ["scalar", "swagger"];
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // if (ByPassRequestLogging
        //     .Any(x => context.Request.Path.Value?.Contains(x, StringComparison.InvariantCultureIgnoreCase) ?? false))
        // {
        //     await next(context);
        //     return;
        // }
        
        logger.LogInformation("Request to the endpoint: {@RequestPath}",
            context.Request.Path.Value);

        await next(context);
        
        logger.LogInformation("Request to the endpoint: {@RequestPath} with status code: {@StatusCode}",
            context.Request.Path.Value, 
            context.Response.StatusCode);
    }
}

public static class RequestMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggingRequests(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}