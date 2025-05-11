namespace DataCat.Server.Api.Endpoints.DataSourceTypes;

public sealed record AddDataSourceTypeRequest(string Name);

public sealed class AddDataSourceType : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/data-source-type/add", async (
                [FromServices] IMediator mediator,
                [FromBody] AddDataSourceTypeRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.DataSourceTypes)
            .HasApiVersion(ApiVersions.V1)
            .Produces<int>()
            .WithCustomProblemDetails();
    }
    
    private static AddDataSourceTypeCommand ToCommand(AddDataSourceTypeRequest request)
    {
        return new AddDataSourceTypeCommand(request.Name);
    }
}