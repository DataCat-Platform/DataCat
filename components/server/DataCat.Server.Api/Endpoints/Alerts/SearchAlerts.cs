namespace DataCat.Server.Api.Endpoints.Alerts;

public sealed class SearchAlerts : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/alert/search", async (
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
            .WithTags(ApiTags.Alerts)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Page<SearchAlertsResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static SearchAlertsQuery ToQuery(string? filter, int page, int pageSize)
        => new(page, pageSize, filter);
}