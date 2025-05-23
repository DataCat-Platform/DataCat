namespace DataCat.Server.Api.Endpoints.Panels;

public sealed record UpdatePanelRequest(
    string Title,
    int Type,
    string RawQuery,
    string DataSourceId,
    string Layout,
    string StyleConfiguration);

public sealed class UpdatePanel : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v{version:apiVersion}/panel/update/{panelId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string panelId,
                [FromBody] UpdatePanelRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request, panelId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Panels)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .WithCustomProblemDetails();
    }

    private static UpdatePanelCommand ToCommand(UpdatePanelRequest request, string panelId)
    {
        return new UpdatePanelCommand
        {
            PanelId = panelId,
            Title = request.Title,
            Type = request.Type,
            DataSourceId = request.DataSourceId,
            Layout = request.Layout,
            RawQuery = request.RawQuery,
            StyleConfiguration = request.StyleConfiguration
        };
    }
}