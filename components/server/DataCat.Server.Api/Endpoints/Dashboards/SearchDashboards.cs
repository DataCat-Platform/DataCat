namespace DataCat.Server.Api.Endpoints.Dashboards;

public sealed class SearchDashboards : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/dashboard/search", async (
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
            .WithTags(ApiTags.Dashboards)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Page<SearchDashboardsResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static SearchDashboardsQuery ToQuery(string? filter, int page, int pageSize)
        => new(page, pageSize, filter);
}