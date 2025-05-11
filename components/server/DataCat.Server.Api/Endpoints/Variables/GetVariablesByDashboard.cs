namespace DataCat.Server.Api.Endpoints.Variables;

public sealed class GetVariablesByDashboard : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/variables/dashboard/{dashboardId:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid dashboardId,
                CancellationToken token = default) =>
            {
                var query = new GetVariablesForDashboardQuery(dashboardId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Variables)
            .HasApiVersion(ApiVersions.V1)
            .Produces<List<VariableResponse>>()
            .WithCustomProblemDetails();
    }
}