namespace DataCat.Server.Api.Endpoints.NotificationChannelGroups;

public sealed class GetNotificationChannelGroupById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/notification-channel-group/{name}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string name,
                CancellationToken token = default) =>
            {
                var query = ToQuery(name);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationChannelGroups)
            .HasApiVersion(ApiVersions.V1)
            .Produces<NotificationChannelResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static GetNotificationChannelGroupQuery ToQuery(string name) => new(name);
}