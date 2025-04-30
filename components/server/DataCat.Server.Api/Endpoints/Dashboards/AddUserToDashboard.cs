using DataCat.Server.Application.Commands.Dashboards.AddUser;

namespace DataCat.Server.Api.Endpoints.Dashboards;

public sealed record AddUserToDashboardRequest(
    string DashboardId,
    string UserId);

public sealed class AddUserToDashboard : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/dashboard/add-user", async (
                [FromBody] AddUserToDashboardRequest request,
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Dashboards)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static AddUserToDashboardCommand ToCommand(AddUserToDashboardRequest request)
    {
        return new AddUserToDashboardCommand
        {
            UserId = request.UserId,
            DashboardId = request.DashboardId,
        };
    } 
}