namespace DataCat.Server.Api.Endpoints.Traces;

public sealed class GetTrace : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/traces/{traceId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string traceId,
                [FromQuery] string dataSourceName,
                CancellationToken token = default) =>
            {
                var result = await mediator.Send(new GetTraceQuery(dataSourceName, traceId), token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Traces)
            .HasApiVersion(ApiVersions.V1)
            .Produces<TraceEntry>()
            .WithCustomProblemDetails();
    }
}