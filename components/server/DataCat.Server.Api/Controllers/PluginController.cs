namespace DataCat.Server.Api.Controllers;

public sealed class PluginController : ApiControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("QUERY IS OK");
    }
}