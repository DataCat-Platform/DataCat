namespace DataCat.Server.Api.Endpoints.NotificationChannels;

public sealed record UpdateNotificationChannelRequest(
    string DestinationName,
    string Settings);

public sealed class UpdateNotificationChannel : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v{version:apiVersion}/notification-channel/{notificationChannelId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string notificationChannelId, 
                [FromBody] UpdateNotificationChannelRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request, notificationChannelId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationChannels)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static UpdateNotificationCommand ToCommand(UpdateNotificationChannelRequest request, string notificationChannelId)
    {
        return new UpdateNotificationCommand
        {
            NotificationChannelId = notificationChannelId,
            DestinationTypeName = request.DestinationName,
            Settings = request.Settings
        };
    }
}