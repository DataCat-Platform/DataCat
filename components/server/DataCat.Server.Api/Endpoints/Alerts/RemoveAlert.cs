namespace DataCat.Server.Api.Endpoints.Alerts;

public sealed class RemoveAlert : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/alert/remove/{alertId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string alertId,
                CancellationToken token = default) =>
            {
                var query = ToCommand(alertId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Alerts)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static RemoveAlertCommand ToCommand(string alertId) => new(alertId);
}