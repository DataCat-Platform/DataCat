namespace DataCat.Server.Api.Endpoints.Panels;

public sealed class GetPanelById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/panel/{id:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid id,
                CancellationToken token = default) =>
            {
                var query = ToQuery(id);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Panels)
            .HasApiVersion(ApiVersions.V1)
            .Produces<GetPanelResponse>()
            .WithCustomProblemDetails();
    }

    private static GetPanelQuery ToQuery(Guid id) => new(id);
}