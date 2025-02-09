namespace DataCat.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected async Task<Result<TResponse>> SendAsync<TResponse>(IRequest<Result<TResponse>> request, CancellationToken token = default)
    {
        return await Mediator.Send(request, token);
    }

    protected async Task<Result> SendAsync(IRequest<Result> request, CancellationToken token = default)
    {
        return await Mediator.Send(request, token);
    }
    
    protected async Task SendAsync(IRequest request, CancellationToken token = default)
    {
        await Mediator.Send(request, token);
    }

    protected IActionResult HandleCustomResponse<T, U>(Result<T> result, Func<Result<T>, U> map)
    {
        if (result.IsFailure)
        {
            var problemDetails = CreateProblemDetails(result.Errors); 
            return BadRequest(problemDetails);
        }
        
        var mapped = map(result);
        return Ok(mapped);
    }
    
    protected IActionResult HandleCustomResponse<T>(Result<T> result)
    {
        if (!result.IsFailure) 
            return Ok(result.Value);
        var problemDetails = CreateProblemDetails(result.Errors); 
        return BadRequest(problemDetails);
    }
    
    protected IActionResult HandleCustomResponse(Result result)
    {
        if (!result.IsFailure) 
            return Ok();
        var problemDetails = CreateProblemDetails(result.Errors); 
        return BadRequest(problemDetails);
    }

    protected static ProblemDetails CreateProblemDetails(object? detail)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Server logic error",
            Detail = "There was an error processing the request",
            Extensions = { ["errors"] = detail }
        };
    }
}