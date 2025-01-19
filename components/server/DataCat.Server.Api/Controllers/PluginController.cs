namespace DataCat.Server.Api.Controllers;

public sealed class PluginController : ApiControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddPlugin([FromForm] AddPluginRequest request)
    {
        var response = await SendAsync(request.ToCommand());
        return response.IsFailure 
            ? BadRequest(response) 
            : Ok(response);
    }
    
    [HttpDelete("remove/{pluginId}")]
    public IActionResult RemovePlugin(string pluginId)
    {
        return Ok("Plugin removed successfully");
    }
    
    [HttpGet("list")]
    public IActionResult GetPlugins()
    {
        // plugin info object
        return Ok(new List<string>());
    }
    
    [HttpGet("status/{pluginId}")]
    public IActionResult GetPluginStatus(string pluginId)
    {
        // new PluginStatus()
        return Ok();
    }
    
    [HttpPut("update/{pluginId}")]
    public IActionResult UpdatePluginConfig(string pluginId, [FromBody] string config) // PluginConfig
    {
        return Ok("Plugin config updated successfully");
    }
    
    [HttpPost("enable/{pluginId}")]
    public IActionResult EnablePlugin(string pluginId)
    {
        return Ok("Plugin enabled successfully");
    }
    
    [HttpPost("disable/{pluginId}")]
    public IActionResult DisablePlugin(string pluginId)
    {
        return Ok("Plugin disabled successfully");
    }
}