using DataCat.Server.Application.Queries.Alerts.Get;

namespace DataCat.Server.Api.Endpoints.Alerts;

public sealed class GetAlertById : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/alert/{id:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid id,
                CancellationToken token = default) =>
            {
                var query = ToQuery(id);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Alerts)
            .HasApiVersion(ApiVersions.V1)
            .Produces<GetAlertResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static GetAlertQuery ToQuery(Guid id) => new(id);
}