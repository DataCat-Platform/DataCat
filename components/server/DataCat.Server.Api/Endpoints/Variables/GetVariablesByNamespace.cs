namespace DataCat.Server.Api.Endpoints.Variables;

public sealed class GetVariablesByNamespace : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/variables/namespace/{namespaceId:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid namespaceId,
                CancellationToken token = default) =>
            {
                var query = new GetVariablesForNamespaceQuery(namespaceId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Variables)
            .HasApiVersion(ApiVersions.V1)
            .Produces<List<VariableResponse>>()
            .WithCustomProblemDetails();
    }
}