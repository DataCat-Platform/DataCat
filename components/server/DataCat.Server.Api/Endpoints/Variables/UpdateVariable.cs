namespace DataCat.Server.Api.Endpoints.Variables;

public sealed record UpdateVariableRequest(
    string Placeholder,
    string Value);

public sealed class UpdateVariable : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v{version:apiVersion}/variable/update/{variableId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid variableId,
                [FromBody] UpdateVariableRequest request,
                CancellationToken token = default) =>
            {
                var command = ToCommand(request, variableId);
                var result = await mediator.Send(command, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Variables)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .WithCustomProblemDetails();
    }

    private static UpdateVariableCommand ToCommand(UpdateVariableRequest request, Guid variableId)
    {
        return new UpdateVariableCommand(
            Id: variableId,
            Placeholder: request.Placeholder,
            Value: request.Value);
    }
}