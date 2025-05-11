namespace DataCat.Server.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    : AbstractExceptionHandlerMiddleware(logger)
{
    protected override (int statusCode, CustomProblemDetails problemDetails) GetSpecificResponse(Exception exception)
    {
        int statusCode;
        CustomProblemDetails problemDetails;
            
        switch (exception)
        {
            case CustomValidationException validationException:
                statusCode = StatusCodes.Status400BadRequest;
                
                var errors = validationException.Failures
                    .GroupBy(f => f.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(f => f.ErrorMessage).ToArray()
                    );
                
                problemDetails = new CustomProblemDetails
                {
                    Status = statusCode,
                    Type = "ValidationFailure",
                    Title = "Validation error",
                    Detail = validationException.Message,
                    Instance = validationException.Message,
                    Errors = errors
                };
                break;
            case FileNotFoundException fileNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                problemDetails = new CustomProblemDetails
                {
                    Status = statusCode,
                    Type = "FileNotFound",
                    Title = "File Not Found",
                    Detail = $"Disk problem. Cannot find file with name {fileNotFoundException.Message}",
                    Instance = fileNotFoundException.Message
                };
                break;
            case DirectoryNotFoundException directoryNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                problemDetails = new CustomProblemDetails
                {
                    Status = statusCode,
                    Type = "FileNotFound",
                    Title = "File Not Found",
                    Detail = $"Disk problem. Cannot directory file with name {directoryNotFoundException.Message}",
                    Instance = directoryNotFoundException.Message
                };
                break;
            case AuthenticationException authenticationException:
                statusCode = StatusCodes.Status401Unauthorized;
                problemDetails = new CustomProblemDetails
                {
                    Status = statusCode,
                    Type = "AuthenticationFailure",
                    Title = "Authentication failed",
                    Detail = authenticationException.Message,
                    Instance = authenticationException.Message
                };
                break;
            case ForbiddenException forbiddenException:
                statusCode = StatusCodes.Status403Forbidden;
                problemDetails = new CustomProblemDetails
                {
                    Status = statusCode,
                    Type = "Forbidden",
                    Title = "Forbidden",
                    Detail = forbiddenException.Message,
                    Instance = forbiddenException.Message
                };
                break;
            case HttpRequestException httpRequestException:
                statusCode = StatusCodes.Status400BadRequest;
                problemDetails = new CustomProblemDetails
                {
                    Status = statusCode,
                    Type = "HttpRequestException",
                    Title = "HTTP Error",
                    Detail = httpRequestException.Message,
                    Instance = httpRequestException.Message,
                };
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "ServerError",
                    Title = "Internal Server Error",
                    Detail = "Some server error. Please try later",
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