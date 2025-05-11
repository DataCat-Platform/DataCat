namespace DataCat.Server.Api.Endpoints.Plugins;

public sealed class SearchPlugins : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/plugin/search", async (
                [FromServices] IMediator mediator,
                [FromBody] SearchFilters filters,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10,
                CancellationToken token = default) =>
            {
                var query = ToQuery(filters, page, pageSize);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Plugins)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Page<SearchPluginsResponse>>()
            .WithCustomProblemDetails();
    }

    private static SearchPluginsQuery ToQuery(SearchFilters filters, int page, int pageSize)
        => new(page, pageSize, filters);
}