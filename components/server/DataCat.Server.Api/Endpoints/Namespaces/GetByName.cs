namespace DataCat.Server.Api.Endpoints.Namespaces;

public sealed class GetNamespaceByName : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/namespace/{name}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string name,
                CancellationToken token = default) =>
            {
                var query = ToQuery(name);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Namespaces)
            .HasApiVersion(ApiVersions.V1)
            .Produces<NamespaceByNameResponse>()
            .WithCustomProblemDetails();
    }

    private static GetNamespaceByNameQuery ToQuery(string name) => new(name);
}