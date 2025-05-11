namespace DataCat.Server.Api.Endpoints.NotificationDestinations;

public sealed class GetNotificationDestinationByName : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/notification-destination/{name}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string name,
                CancellationToken token = default) =>
            {
                var query = ToQuery(name);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationDestination)
            .HasApiVersion(ApiVersions.V1)
            .Produces<GetNotificationDestinationResponse>()
            .WithCustomProblemDetails();
    }

    private static GetNotificationDestinationQuery ToQuery(string name) => new(name);
}