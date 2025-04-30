using DataCat.Server.Application.Queries.Plugins.Get;

namespace DataCat.Server.Api.Endpoints.Plugins;

public sealed class GetPluginById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/plugin/{id:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid id,
                CancellationToken token = default) =>
            {
                var query = ToQuery(id);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Plugins)
            .HasApiVersion(ApiVersions.V1)
            .Produces<GetPluginResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static GetPluginQuery ToQuery(Guid id) => new(id);
}