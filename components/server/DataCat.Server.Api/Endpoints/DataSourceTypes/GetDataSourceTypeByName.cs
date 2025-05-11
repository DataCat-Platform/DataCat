namespace DataCat.Server.Api.Endpoints.DataSourceTypes;

public sealed class GetDataSourceTypeByName : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/data-source-type/{name}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string name,
                CancellationToken token = default) =>
            {
                var query = ToQuery(name);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.DataSourceTypes)
            .HasApiVersion(ApiVersions.V1)
            .Produces<GetDataSourceTypeResponse>()
            .WithCustomProblemDetails();
    }

    private static GetDataSourceTypeQuery ToQuery(string name) => new(name);
}