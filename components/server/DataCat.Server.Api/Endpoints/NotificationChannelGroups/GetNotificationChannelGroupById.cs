namespace DataCat.Server.Api.Endpoints.NotificationChannelGroups;

public sealed class GetNotificationChannelGroupById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/notification-channel-group/{id:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid id,
                CancellationToken token = default) =>
            {
                var query = ToQuery(id);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationChannelGroups)
            .HasApiVersion(ApiVersions.V1)
            .Produces<NotificationChannelGroupResponse>()
            .WithCustomProblemDetails();
    }

    private static GetNotificationChannelGroupQuery ToQuery(Guid name) => new(name);
}