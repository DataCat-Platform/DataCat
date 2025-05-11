namespace DataCat.Server.Api.Endpoints.DataSourceTypes;

public sealed class GetAllDataSources : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/data-source-type/get-all", async (
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToQuery();
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.DataSourceTypes)
            .HasApiVersion(ApiVersions.V1)
            .Produces<List<GetDataSourceTypeResponse>>()
            .WithCustomProblemDetails();
    }

    private static GetAllDataSourceTypesQuery ToQuery() => new();
}