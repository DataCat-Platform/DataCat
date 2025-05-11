namespace DataCat.Server.Api.Middlewares;

public abstract class AbstractExceptionHandlerMiddleware(ILogger<AbstractExceptionHandlerMiddleware> logger)
    : IMiddleware
{
    protected abstract (int statusCode, CustomProblemDetails problemDetails) GetSpecificResponse(Exception exception);

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError("Error while executing request: {@RequestPath} with error message: {@ErrorMessage}",
                context.Request.Path.Value,
                e.Message);
            var (status, problemDetails) = GetSpecificResponse(e);
            
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}