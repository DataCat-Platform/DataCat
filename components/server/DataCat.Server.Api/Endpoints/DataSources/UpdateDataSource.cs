using DataCat.Server.Application.Commands.DataSources.UpdateConnectionString;

namespace DataCat.Server.Api.Endpoints.DataSources;

public sealed record UpdateDataSourceRequest(string ConnectionString);

public sealed class UpdateDataSource : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v{version:apiVersion}/data-source/update-connection-string/{dataSourceId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string dataSourceId, 
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
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static UpdateConnectionStringDataSourceCommand ToCommand(UpdateDataSourceRequest request, string dataSourceId)
    {
        return new UpdateConnectionStringDataSourceCommand
        {
            DataSourceId = dataSourceId,
            ConnectionString = request.ConnectionString
        };
    }
}