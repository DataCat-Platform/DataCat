namespace DataCat.Server.Api.Endpoints.Meta;

public sealed class GetDatabaseScheme : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/meta/database-schema", (
                [FromServices] IDatabaseAssemblyScanner databaseAssemblyScanner) =>
            {
                var result = databaseAssemblyScanner.GetDatabaseSchema(); 
                return Results.Ok(result.Select(x => new SchemaResponse
                {
                    Migration = x.MigrationName,
                    UpSql = x.UpSql,
                    DownSql = x.DownSql,
                }));
            })
            .HasApiVersion(ApiVersions.V1)
            .Produces<IEnumerable<SchemaResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}