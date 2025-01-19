namespace DataCat.Server.Api.Middlewares;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    : AbstractExceptionHandlerMiddleware(logger)
{
    protected override (int statusCode, ProblemDetails problemDetails) GetSpecificResponse(Exception exception)
    {
        int statusCode;
        ProblemDetails problemDetails;
            
        switch (exception)
        {
            case CustomValidationException validationException:
                statusCode = StatusCodes.Status400BadRequest;
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Type = "ValidationFailure",
                    Title = "Validation error",
                    Detail = "One or more validation errors occurred",
                    Extensions =
                    {
                        ["errors"] = validationException.Failures
                    }
                };
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                problemDetails = new ProblemDetails()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "ServerError",
                    Title = "Internal Server Error",
                    Detail = "Some server error. Please try later"
                };
                break;
        }
        
        return (statusCode, problemDetails);
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}