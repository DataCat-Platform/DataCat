namespace DataCat.Server.Api.Endpoints.NotificationChannelGroups;

public sealed class GetAllNotificationChannelGroups : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/notification-channel-group/get-all", async (
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToQuery();
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationChannelGroups)
            .HasApiVersion(ApiVersions.V1)
            .Produces<List<NotificationChannelResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static GetAllNotificationChannelGroupsQuery ToQuery() => new();
}