namespace DataCat.Server.Api.Controllers;

public sealed class DebugController(IMigrationRunnerFactory runnerFactory)
    : ApiControllerBase
{
    [HttpGet("rollback/{steps:int}")]
    public async Task<IActionResult> RollbackMigrations(int steps)
    {
        var runner = runnerFactory.CreateMigrationRunner();
        await runner.RollbackLastMigrationAsync(steps);
        return Ok();
    }
}