namespace DataCat.Server.Api.Endpoints.Variables;

public sealed class RemoveVariable : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/variable/remove/{variableId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid variableId,
                CancellationToken token = default) =>
            {
                var command = ToCommand(variableId);
                var result = await mediator.Send(command, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Variables)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static RemoveVariableCommand ToCommand(Guid variableId) => new(variableId);
}