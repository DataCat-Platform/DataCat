namespace DataCat.Server.Api.Endpoints.Panels;

public sealed class GetPanelTypes : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/panel/types", async (
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToQuery();
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Panels)
            .HasApiVersion(ApiVersions.V1)
            .Produces<List<GetPanelTypesResponse>>()
            .WithCustomProblemDetails();
    }

    private static GetPanelTypesQuery ToQuery() => new();
}