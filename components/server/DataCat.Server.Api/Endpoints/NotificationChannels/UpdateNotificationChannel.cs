namespace DataCat.Server.Api.Endpoints.NotificationChannels;

public sealed record UpdateNotificationChannelRequest(
    string DestinationName,
    string Settings);

public sealed class UpdateNotificationChannel : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v{version:apiVersion}/notification-channel/{notificationChannelId:int}", async (
                [FromServices] IMediator mediator,
                [FromRoute] int notificationChannelId, 
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
            .WithCustomProblemDetails();
    }

    private static UpdateNotificationCommand ToCommand(UpdateNotificationChannelRequest request, int notificationChannelId)
    {
        return new UpdateNotificationCommand
        {
            NotificationChannelId = notificationChannelId,
            DestinationTypeName = request.DestinationName,
            Settings = request.Settings
        };
    }
}