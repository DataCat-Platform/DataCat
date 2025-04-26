namespace DataCat.Server.Api.Endpoints.DataSourceTypes;

public sealed class RemoveDataSourceType : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/datasource/remove/{name}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string name,
                CancellationToken token = default) =>
            {
                var query = ToCommand(name);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.DataSources)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static RemoveDataSourceTypeCommand ToCommand(string name) => new(name);
}