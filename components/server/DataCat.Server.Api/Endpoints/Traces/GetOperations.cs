namespace DataCat.Server.Api.Endpoints.Traces;

public sealed class GetOperations : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/traces/operations", async (
                [FromServices] IMediator mediator,
                [FromQuery] string dataSourceName,
                [FromQuery] string serviceName,
                CancellationToken token = default) =>
            {
                var result = await mediator.Send(new GetOperationsQuery(dataSourceName, serviceName), token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Traces)
            .HasApiVersion(ApiVersions.V1)
            .Produces<IEnumerable<string>>()
            .WithCustomProblemDetails();
    }
}