using DataCat.Server.Application.Commands.Plugins.ToggleStatus;

namespace DataCat.Server.Api.Endpoints.Plugins;

public sealed class TogglePluginStatus : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/plugin/{pluginId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string pluginId,
                [FromQuery] bool isActive = false,
                CancellationToken token = default) =>
            {
                var query = ToCommand(pluginId, isActive);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Plugins)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
    
    private static ToggleStatusCommand ToCommand(string pluginId, bool isActive) 
        => new() { PluginId = pluginId, ToggleStatus = isActive ? ToggleStatus.Active : ToggleStatus.InActive };
}