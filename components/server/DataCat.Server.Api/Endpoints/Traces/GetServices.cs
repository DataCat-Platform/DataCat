namespace DataCat.Server.Api.Endpoints.Traces;

public sealed class GetServices : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/traces/services", async (
                [FromServices] IMediator mediator,
                [FromQuery] string dataSourceName,
                CancellationToken token = default) =>
            {
                var result = await mediator.Send(new GetServicesQuery(dataSourceName), token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Traces)
            .HasApiVersion(ApiVersions.V1)
            .Produces<IEnumerable<string>>()
            .WithCustomProblemDetails();
    }
}