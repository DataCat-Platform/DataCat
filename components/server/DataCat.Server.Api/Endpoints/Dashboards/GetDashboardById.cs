namespace DataCat.Server.Api.Endpoints.Dashboards;

public sealed class GetDashboardById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/dashboard/{id:guid}", async (
                Guid id,
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                var query = ToQuery(id);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Dashboards)
            .HasApiVersion(ApiVersions.V1)
            .Produces<DashboardResponse>()
            .WithCustomProblemDetails();
    }

    private static GetDashboardQuery ToQuery(Guid id) => new(id);
}