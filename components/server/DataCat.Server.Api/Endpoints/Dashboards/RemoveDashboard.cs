namespace DataCat.Server.Api.Endpoints.Dashboards;

public sealed class RemoveDashboard : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/dashboard/remove/{dashboardId}", async (
                [FromRoute] string dashboardId,
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToCommand(dashboardId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Dashboards)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .WithCustomProblemDetails();
    }

    private static RemoveDashboardCommand ToCommand(string dashboardId) => new(dashboardId);
}