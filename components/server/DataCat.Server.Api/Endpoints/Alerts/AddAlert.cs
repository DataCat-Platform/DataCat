namespace DataCat.Server.Api.Endpoints.Alerts;

public sealed record AddAlertRequest(
    string? Description,
    string RawQuery,
    string DataSourceId,
    string NotificationChannelGroupName,
    TimeSpan WaitTimeBeforeAlerting,
    TimeSpan RepeatInterval,
    List<string> Tags);

public sealed class AddAlert : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/alert/add", async (
                [FromBody] AddAlertRequest request,
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Alerts)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Guid>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static AddAlertCommand ToCommand(AddAlertRequest request)
    {
        return new AddAlertCommand
        {
            Description = request.Description,
            RawQuery = request.RawQuery,
            DataSourceId = request.DataSourceId,
            NotificationChannelGroupName = request.NotificationChannelGroupName,
            WaitTimeBeforeAlerting = request.WaitTimeBeforeAlerting,
            RepeatInterval = request.RepeatInterval,
            Tags = request.Tags ?? []
        };
    }
}