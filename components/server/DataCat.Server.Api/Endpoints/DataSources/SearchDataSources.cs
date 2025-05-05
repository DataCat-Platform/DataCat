namespace DataCat.Server.Api.Endpoints.DataSources;

public sealed class SearchDataSources : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/data-source/search", async (
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
            .WithTags(ApiTags.DataSources)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Page<SearchDataSourcesResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static SearchDataSourcesQuery ToQuery(SearchFilters filters, int page, int pageSize)
        => new(page, pageSize, filters);
}