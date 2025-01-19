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
}