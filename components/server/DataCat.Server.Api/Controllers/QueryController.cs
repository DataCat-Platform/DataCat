namespace DataCat.Server.Api.Controllers;

public sealed class QueryController : ApiControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("QUERY IS OK");
    }
}

