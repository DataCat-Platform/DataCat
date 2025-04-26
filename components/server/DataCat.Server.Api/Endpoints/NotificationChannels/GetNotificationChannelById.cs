using DataCat.Server.Application.Queries.NotificationChannels.Get;

namespace DataCat.Server.Api.Endpoints.NotificationChannels;

public sealed class GetNotificationChannelById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/notification-channel/{id:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid id,
                CancellationToken token = default) =>
            {
                var query = ToQuery(id);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationChannels)
            .HasApiVersion(ApiVersions.V1)
            .Produces<GetNotificationChannelResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static GetNotificationChannelQuery ToQuery(Guid id) => new(id);
}