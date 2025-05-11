namespace DataCat.Server.Api.Endpoints.Meta;

public sealed record SchemaResponse(string Migration, dynamic UpSql, dynamic DownSql);

public sealed class GetDatabaseScheme : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/meta/database-schema", (
                [FromServices] IDatabaseAssemblyScanner databaseAssemblyScanner) =>
            {
                var result = databaseAssemblyScanner.GetDatabaseSchema(); 
                return Results.Ok(result.Select(x => new SchemaResponse(x.MigrationName, x.UpSql, x.DownSql)));
            })
            .HasApiVersion(ApiVersions.V1)
            .Produces<IEnumerable<SchemaResponse>>()
            .WithCustomProblemDetails();
    }
}