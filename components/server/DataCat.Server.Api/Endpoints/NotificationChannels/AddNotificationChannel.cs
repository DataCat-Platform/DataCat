namespace DataCat.Server.Api.Endpoints.NotificationChannels;

public sealed record AddNotificationChannelRequest(
    int DestinationId,
    string Settings);

public sealed class AddNotificationChannel : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/notification-channel/add", async (
                [FromServices] IMediator mediator,
                [FromBody] AddNotificationChannelRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationChannels)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Guid>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
    
    private static AddNotificationCommand ToCommand(AddNotificationChannelRequest request)
    {
        return new AddNotificationCommand
        {
            DestinationType = request.DestinationId,
            Settings = request.Settings
        };
    }
}