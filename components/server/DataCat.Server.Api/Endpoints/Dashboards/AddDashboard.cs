namespace DataCat.Server.Api.Endpoints.Dashboards;

public sealed record AddDashboardRequest(
    string Name,
    string? Description,
    Guid NamespaceId,
    List<string> Tags);

public sealed class AddDashboard : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/dashboard/add", async (
                [FromBody] AddDashboardRequest request,
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Dashboards)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Guid>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static AddDashboardCommand ToCommand(AddDashboardRequest request)
    {
        return new AddDashboardCommand
        {
            Name = request.Name,
            Description = request.Description,
            Tags = request.Tags,
            NamespaceId = request.NamespaceId,
        };
    }
}