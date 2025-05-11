namespace DataCat.Server.Api.Endpoints;

public abstract class ApiEndpointBase
{
    public abstract void MapEndpoint(IEndpointRouteBuilder app);

    protected IResult HandleCustomResponse<T, U>(Result<T> result, Func<Result<T>, U> map)
    {
        if (result.IsFailure)
        {
            var problemDetails = CreateProblemDetails(result.Errors); 
            return Results.BadRequest(problemDetails);
        }
        
        var mapped = map(result);
        return Results.Ok(mapped);
    }
    
    protected IResult HandleCustomResponse<T>(Result<T> result)
    {
        if (!result.IsFailure)
        {
            return Results.Ok(result.Value);
        }
        
        var problemDetails = CreateProblemDetails(result.Errors); 
        return Results.BadRequest(problemDetails);
    }
    
    protected IResult HandleCustomResponse(Result result)
    {
        if (!result.IsFailure)
        {
            return Results.Ok();
        }
            
        var problemDetails = CreateProblemDetails(result.Errors); 
        return Results.BadRequest(problemDetails);
    }

    private static CustomProblemDetails CreateProblemDetails(List<ErrorInfo>? detail)
    {
        var errorsDict = new Dictionary<string, string[]>();

        if (detail is not null && detail.Count > 0)
        {
            foreach (var error in detail)
            {
                var key = string.IsNullOrWhiteSpace(error.ErrorCode) ? "General" : error.ErrorCode;

                if (!errorsDict.TryGetValue(key, out var list))
                {
                    list = [];
                    errorsDict[key] = list;
                }

                errorsDict[key] = errorsDict[key].Append(error.ErrorMessage).ToArray();
            }
        }
        
        return new CustomProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Server logic error",
            Detail = "There was an error processing the request",
            Instance = "There was an error processing the request",
            Errors = errorsDict
        };
    }
}