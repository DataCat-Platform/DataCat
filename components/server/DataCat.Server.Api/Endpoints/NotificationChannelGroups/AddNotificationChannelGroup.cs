namespace DataCat.Server.Api.Endpoints.NotificationChannelGroups;

public sealed record AddNotificationChannelGroupRequest(string Name);

public sealed class AddNotificationChannelGroup : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/notification-channel-group/add", async (
                [FromServices] IMediator mediator,
                [FromBody] AddNotificationChannelGroupRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationChannelGroups)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Guid>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
    
    private static AddNotificationChannelGroupCommand ToCommand(AddNotificationChannelGroupRequest request)
    {
        return new AddNotificationChannelGroupCommand(request.Name);
    }
}