namespace DataCat.Server.Api.Endpoints.ExternalRoleMapping;

public sealed record AddExternalRoleMappingRequest(
    string ExternalRole,
    string NamespaceId,
    int RoleId);

public sealed class AddExternalRoleMapping : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/external-role", async (
                [FromBody] AddExternalRoleMappingRequest request,
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.ExternalRoleMappings)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .WithCustomProblemDetails();
    }

    private static AddExternalRoleMappingCommand ToCommand(AddExternalRoleMappingRequest request)
    {
        return new AddExternalRoleMappingCommand(request.ExternalRole, request.NamespaceId, request.RoleId);
    }
}