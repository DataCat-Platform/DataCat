using DataCat.Server.Application.Queries.Plugins.Search;

namespace DataCat.Server.Api.Endpoints.Plugins;

public sealed class SearchPlugins : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/plugin/search", async (
                [FromServices] IMediator mediator,
                [FromQuery] string? filter = null,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10,
                CancellationToken token = default) =>
            {
                var query = ToQuery(filter, page, pageSize);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Plugins)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Page<SearchPluginsResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static SearchPluginsQuery ToQuery(string? filter, int page, int pageSize)
        => new(page, pageSize, filter);
}