namespace DataCat.Server.Api.Endpoints.DataSources;

public sealed class GetDataSourcesById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/datasources/{id:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid id,
                CancellationToken token = default) =>
            {
                var query = ToQuery(id);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Alerts)
            .HasApiVersion(ApiVersions.V1)
            .Produces<GetDataSourceResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static GetDataSourceQuery ToQuery(Guid id) => new(id);
}