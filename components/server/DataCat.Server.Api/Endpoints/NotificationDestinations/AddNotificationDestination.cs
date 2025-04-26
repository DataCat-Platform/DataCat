namespace DataCat.Server.Api.Endpoints.NotificationDestinations;

public sealed record AddNotificationDestinationRequest(string Name);

public sealed class AddNotificationDestination : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/notification-destination/add", async (
                [FromServices] IMediator mediator,
                [FromBody] AddNotificationDestinationRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.NotificationDestination)
            .HasApiVersion(ApiVersions.V1)
            .Produces<int>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
    
    private static AddNotificationDestinationCommand ToCommand(AddNotificationDestinationRequest request)
    {
        return new AddNotificationDestinationCommand(request.Name);
    }
}