namespace DataCat.Server.Api.Endpoints.Variables;

public sealed record AddVariableRequest(
    string Placeholder,
    string Value,
    Guid NamespaceId,
    Guid? DashboardId = null);

public sealed class AddVariable : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/variable/add", async (
                [FromServices] IMediator mediator,
                [FromBody] AddVariableRequest request,
                CancellationToken token = default) =>
            {
                var command = ToCommand(request);
                var result = await mediator.Send(command, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Variables)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Guid>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static AddVariableCommand ToCommand(AddVariableRequest request)
    {
        return new AddVariableCommand(
            Placeholder: request.Placeholder,
            Value: request.Value,
            NamespaceId: request.NamespaceId,
            DashboardId: request.DashboardId);
    }
}