namespace DataCat.Server.Api.Endpoints.NotificationChannels;

public sealed class GetNotificationChannelById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/notification-channel/{id:int}", async (
                [FromServices] IMediator mediator,
                [FromRoute] int id,
                CancellationToken token = default) =>
            {
                var query = ToQuery(id);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationChannels)
            .HasApiVersion(ApiVersions.V1)
            .Produces<NotificationChannelResponse>()
            .WithCustomProblemDetails();
    }

    private static GetNotificationChannelQuery ToQuery(int id) => new(id);
}