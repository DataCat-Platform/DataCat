namespace DataCat.Server.Api.Endpoints.Alerts;

public sealed class GetAlertCounters : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/alert/get-counters", async (
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToQuery();
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Alerts)
            .HasApiVersion(ApiVersions.V1)
            .Produces<List<AlertCounterResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static GetAlertCountersQuery ToQuery() => new();
}