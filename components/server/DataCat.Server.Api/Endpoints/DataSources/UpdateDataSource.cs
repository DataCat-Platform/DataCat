namespace DataCat.Server.Api.Endpoints.DataSources;

public sealed record UpdateDataSourceRequest(string ConnectionString);

public sealed class UpdateDataSource : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v{version:apiVersion}/data-source/update-connection-string/{dataSourceId:guid}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid dataSourceId, 
                [FromBody] UpdateDataSourceRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request, dataSourceId);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.DataSources)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .WithCustomProblemDetails();
    }

    private static UpdateConnectionStringDataSourceCommand ToCommand(UpdateDataSourceRequest request, Guid dataSourceId)
    {
        return new UpdateConnectionStringDataSourceCommand
        {
            Id = dataSourceId,
            ConnectionString = request.ConnectionString
        };
    }
}