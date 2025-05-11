namespace DataCat.Server.Api.Endpoints.Meta;

public sealed class GetSqlQueries : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/meta/sql-queries", (
                [FromServices] IDatabaseAssemblyScanner databaseAssemblyScanner) =>
            {
                var result = databaseAssemblyScanner.GetSqlQueries(); 
                return Results.Ok(result);
            })
            .HasApiVersion(ApiVersions.V1)
            .Produces<List<string>>()
            .WithCustomProblemDetails();
    }
}