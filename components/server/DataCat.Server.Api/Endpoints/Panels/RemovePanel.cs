using DataCat.Server.Application.Commands.Panels.Remove;

namespace DataCat.Server.Api.Endpoints.Panels;

public sealed class RemovePanel : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/panel/remove/{panelId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string panelId,
                CancellationToken token = default) =>
            {
                var query = ToCommand(panelId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Panels)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static RemovePanelCommand ToCommand(string panelId) => new(panelId);
}