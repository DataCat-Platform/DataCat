namespace DataCat.Server.Api.Endpoints.Plugins;

public sealed record AddPluginRequest(
    IFormFile File,
    string Name,
    string Version,
    string Author,
    string? Description = null,
    string? Settings = null);


public sealed class AddPlugin : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/plugin/add", async (
                [FromServices] IMediator mediator,
                [FromForm] AddPluginRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Plugins)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Guid>()
            .WithCustomProblemDetails();
    }
    
    private static AddPluginCommand ToCommand(AddPluginRequest request)
    {
        return new AddPluginCommand
        {
            File = request.File,
            Name = request.Name,
            Version = request.Version,
            Description = request.Description,
            Author = request.Author,
            Settings = request.Settings
        };
    }
}