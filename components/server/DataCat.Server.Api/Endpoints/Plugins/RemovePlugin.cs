using DataCat.Server.Application.Commands.Plugins.Remove;

namespace DataCat.Server.Api.Endpoints.Plugins;

public sealed class RemovePlugin : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/plugin/remove/{pluginId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string pluginId,
                CancellationToken token = default) =>
            {
                var query = ToCommand(pluginId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Plugins)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static RemovePluginCommand ToCommand(string dataSourceId) => new(dataSourceId);
}