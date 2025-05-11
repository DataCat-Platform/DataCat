namespace DataCat.Server.Api.Endpoints.Dashboards;

public sealed class SearchDashboards : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/dashboard/search", async (
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
            .WithTags(ApiTags.Dashboards)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Page<SearchDashboardsResponse>>()
            .WithCustomProblemDetails();
    }

    private static SearchDashboardsQuery ToQuery(SearchFilters filters, int page, int pageSize)
        => new(page, pageSize, filters);
}