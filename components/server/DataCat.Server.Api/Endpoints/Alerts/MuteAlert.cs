using DataCat.Server.Application.Commands.Alerts.Mute;

namespace DataCat.Server.Api.Endpoints.Alerts;

public sealed class MuteAlert : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/alert/mute/{alertId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string alertId,
                [FromQuery] TimeSpan nextExecutionTime,
                CancellationToken token = default) =>
            {
                var query = ToCommand(alertId, nextExecutionTime);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Alerts)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Guid>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static MuteAlertCommand ToCommand(string alertId, TimeSpan nextExecutionTime)
        => new(alertId, nextExecutionTime);
}