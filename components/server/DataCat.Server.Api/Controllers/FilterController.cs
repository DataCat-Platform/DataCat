namespace DataCat.Server.Api.Controllers;

public sealed class FilterController : ApiControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("QUERY IS OK");
    }
}