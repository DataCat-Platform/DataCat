namespace DataCat.Server.Api.Endpoints.Alerts;

public sealed class SearchAlerts : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/alert/search", async (
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
            .WithTags(ApiTags.Alerts)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Page<SearchAlertsResponse>>()
            .WithCustomProblemDetails();
    }

    private static SearchAlertsQuery ToQuery(SearchFilters filters, int page, int pageSize)
        => new(page, pageSize, filters);
}