namespace DataCat.Server.Api.Endpoints.DataSources;

public sealed class RemoveDataSource : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/datasource/remove/{dataSourceId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string dataSourceId,
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

    private static RemoveDataSourceCommand ToCommand(string dataSourceId) => new(dataSourceId);
}