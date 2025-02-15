namespace DataCat.Server.Api.Controllers;

public class MetaController(IDatabaseAssemblyScanner databaseAssemblyScanner) : ApiControllerBase
{
    [HttpGet("database-schema")]
    public IActionResult GetDatabaseSchema()
    {
        var result = databaseAssemblyScanner.GetDatabaseSchema(); 
        return Ok(result.Select(x => new SchemaResponse
        {
            Migration = x.MigrationName,
            UpSql = x.UpSql,
            DownSql = x.DownSql,
        }));
    }
    
    [HttpGet("sql-queries")]
    public IActionResult GetSqlQueries()
    {
        var result = databaseAssemblyScanner.GetSqlQueries(); 
        return Ok(result);
    }
}