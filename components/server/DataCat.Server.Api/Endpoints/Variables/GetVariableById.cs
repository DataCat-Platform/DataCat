namespace DataCat.Server.Api.Endpoints.Variables;

public sealed class GetVariableById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/variable/{id:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid id,
                CancellationToken token = default) =>
            {
                var query = ToQuery(id);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Variables)
            .HasApiVersion(ApiVersions.V1)
            .Produces<VariableResponse>()
            .WithCustomProblemDetails();
    }

    private static GetVariableByIdQuery ToQuery(Guid id) => new(id);
}