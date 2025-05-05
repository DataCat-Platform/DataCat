namespace DataCat.Server.Api.Endpoints.Dashboards;

public sealed record UpdateDashboardRequest(string Name, string? Description);

public sealed class UpdateDashboard : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v{version:apiVersion}/dashboard/update/{dashboardId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string dashboardId, 
                [FromBody] UpdateDashboardRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request, dashboardId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Dashboards)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static UpdateDashboardCommand ToCommand(UpdateDashboardRequest request, string dashboardId)
    {
        return new UpdateDashboardCommand
        {
            DashboardId = dashboardId,
            Name = request.Name,
            Description = request.Description
        };
    }
}