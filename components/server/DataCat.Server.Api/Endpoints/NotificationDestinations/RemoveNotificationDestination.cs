namespace DataCat.Server.Api.Endpoints.NotificationDestinations;

public sealed class RemoveNotificationDestination : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/notification-destination/remove/{name}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string name,
                CancellationToken token = default) =>
            {
                var query = ToCommand(name);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationDestination)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static RemoveNotificationDestinationCommand ToCommand(string name) => new(name);
}