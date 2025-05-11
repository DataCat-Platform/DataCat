namespace DataCat.Server.Api.Endpoints.DataSources;

public sealed record AddDataSourceRequest(
    string UniqueName,
    string DataSourceType,
    string ConnectionString,
    DataSourceKind Purpose);

public sealed class AddDataSource : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/data-source/add", async (
                [FromServices] IMediator mediator,
                [FromBody] AddDataSourceRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.DataSources)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Guid>()
            .WithCustomProblemDetails();
    }
    
    private static AddDataSourceCommand ToCommand(AddDataSourceRequest request)
    {
        return new AddDataSourceCommand
        {
            Name = request.UniqueName,
            ConnectionString = request.ConnectionString,
            DataSourceType = request.DataSourceType,
            Purpose = request.Purpose
        };
    }
}