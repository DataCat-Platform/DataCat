namespace DataCat.Server.Api.Endpoints.Plugins;

public sealed record UpdatePluginRequest(
    string Description,
    string? Settings);

public sealed class UpdatePlugin : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v{version:apiVersion}/plugin/update/{pluginId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string pluginId, 
                [FromBody] UpdatePluginRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request, pluginId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Plugins)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .WithCustomProblemDetails();
    }

    private static UpdatePluginCommand ToCommand(UpdatePluginRequest request, string pluginId)
    {
        return new UpdatePluginCommand
        {
            PluginId = pluginId,
            Description = request.Description,
            Settings = request.Settings
        };
    }
}