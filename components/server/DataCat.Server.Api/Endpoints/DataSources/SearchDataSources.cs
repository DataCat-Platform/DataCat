using DataCat.Server.Application.Queries.DataSources.Search;

namespace DataCat.Server.Api.Endpoints.DataSources;

public sealed class SearchDataSources : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/data-source/search", async (
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
            .WithTags(ApiTags.DataSources)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Page<SearchDataSourcesResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static SearchDataSourcesQuery ToQuery(string? filter, int page, int pageSize)
        => new(page, pageSize, filter);
}