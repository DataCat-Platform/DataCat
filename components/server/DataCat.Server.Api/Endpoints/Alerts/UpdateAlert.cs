using DataCat.Server.Application.Commands.Alerts.Update;

namespace DataCat.Server.Api.Endpoints.Alerts;

public sealed record UpdateAlertRequest(
    string? Description,
    string RawQuery,
    string DataSourceId,
    string NotificationChannelId);

public sealed class UpdateAlert : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v{version:apiVersion}/alert/update/{alertId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string alertId,
                [FromBody] UpdateAlertRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request, alertId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Alerts)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static UpdateAlertQueryCommand ToCommand(UpdateAlertRequest request, string alertId)
    {
        return new UpdateAlertQueryCommand
        {
            AlertId = alertId,
            Description = request.Description,
            RawQuery = request.RawQuery,
            DataSourceId = request.DataSourceId,
            NotificationChannelId = request.NotificationChannelId
        };
    }
}