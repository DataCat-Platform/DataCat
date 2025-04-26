using DataCat.Server.Application.Commands.NotificationChannels.Remove;

namespace DataCat.Server.Api.Endpoints.NotificationChannels;

public sealed class RemoveNotificationChannel : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/notification-channel/remove/{notificationChannelId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string notificationChannelId,
                CancellationToken token = default) =>
            {
                var query = ToCommand(notificationChannelId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationChannels)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static RemoveNotificationCommand ToCommand(string dataSourceId) => new(dataSourceId);
}