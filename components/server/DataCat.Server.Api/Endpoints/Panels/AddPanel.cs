namespace DataCat.Server.Api.Endpoints.Panels;

public sealed record AddPanelRequest(
    string Title,
    int Type,
    string RawQuery,
    string DataSourceId,
    string Layout,
    string DashboardId,
    string StyleConfiguration);

public sealed class AddPanel : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/panel/add", async (
                [FromServices] IMediator mediator,
                [FromBody] AddPanelRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Panels)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Guid>()
            .WithCustomProblemDetails();
    }

    private static AddPanelCommand ToCommand(AddPanelRequest request)
    {
        return new AddPanelCommand
        {
            Title = request.Title,
            Type = request.Type,
            DashboardId = request.DashboardId,
            DataSourceId = request.DataSourceId,
            Layout = request.Layout,
            RawQuery = request.RawQuery,
            StyleConfiguration = request.StyleConfiguration
        };
    }
}