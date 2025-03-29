namespace DataCat.Server.Api.Endpoints.Debug;

public sealed class RollbackMigrations : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/debug/rollback/{steps:int}", async (
                [FromRoute] int steps,
                [FromServices] IMigrationRunnerFactory runnerFactory,
                CancellationToken token = default) =>
            {
                var runner = runnerFactory.CreateMigrationRunner();
                await runner.RollbackLastMigrationAsync(steps, token);
                return Results.Ok();
            })
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}