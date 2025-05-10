namespace DataCat.Server.Api.Endpoints.Namespaces;

public sealed class GetAvailableNamespaces : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/namespace/get-available", async (
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = new GetAvailableNamespacesQuery();
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Namespaces)
            .HasApiVersion(ApiVersions.V1)
            .Produces<List<GetAvailableNamespaceResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}