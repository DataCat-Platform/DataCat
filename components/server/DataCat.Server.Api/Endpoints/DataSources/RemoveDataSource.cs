namespace DataCat.Server.Api.Endpoints.DataSources;

public sealed class RemoveDataSource : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/data-source/remove/{dataSourceId:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid dataSourceId,
                CancellationToken token = default) =>
            {
                var query = ToCommand(dataSourceId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.DataSources)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static RemoveDataSourceCommand ToCommand(Guid dataSourceId) => new(dataSourceId);
}