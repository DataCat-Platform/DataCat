using System.Security.Authentication;

namespace DataCat.Server.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
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
            case FileNotFoundException fileNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Type = "FileNotFound",
                    Title = "File Not Found",
                    Detail = $"Disk problem. Cannot find file with name {fileNotFoundException.Message}",
                    Extensions =
                    {
                        ["errors"] = fileNotFoundException.Message
                    }
                };
                break;
            case DirectoryNotFoundException directoryNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Type = "FileNotFound",
                    Title = "File Not Found",
                    Detail = $"Disk problem. Cannot directory file with name {directoryNotFoundException.Message}",
                    Extensions =
                    {
                        ["errors"] = directoryNotFoundException.Message
                    }
                };
                break;
            case AuthenticationException authenticationException:
                statusCode = StatusCodes.Status401Unauthorized;
                problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Type = "AuthenticationFailure",
                    Title = "Authentication failed",
                    Detail = authenticationException.Message,
                    Extensions =
                    {
                        ["errors"] = authenticationException.Message
                    }
                };
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "ServerError",
                    Title = "Internal Server Error",
                    Detail = "Some server error. Please try later",
                    Extensions =
                    {
                        ["errors"] = exception.Message
                    }
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