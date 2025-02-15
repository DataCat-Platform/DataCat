namespace DataCat.Server.Api.Controllers;

public sealed class PluginController : ApiControllerBase
{
    [HttpPost("add")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPlugin([FromForm] AddPluginRequest request)
    {
        var response = await SendAsync(request.ToAddCommand());
        return HandleCustomResponse(response);
    }
    
    [HttpDelete("remove/{pluginId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemovePlugin([FromRoute] string pluginId)
    {
        var command = new RemovePluginCommand(pluginId);
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
    
    [HttpGet("search")]
    [ProducesResponseType(typeof(FullPluginResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlugins(
        [FromQuery] string? filter = null, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchPluginsQuery(page, pageSize, filter);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.Select(x => x.ToResponse()));
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FullPluginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlugin(Guid id)
    {
        var query = new GetPluginQuery(id);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.ToResponse());
    }
    
    [HttpPut("update/{pluginId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePluginConfig([FromRoute] string pluginId, [FromBody] UpdatePluginRequest request)
    {
        var query = request.ToUpdateCommand(pluginId);
        var response = await SendAsync(query);
        return HandleCustomResponse(response);
    }
    
    [HttpPost("enable/{pluginId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EnablePlugin([FromRoute] string pluginId)
    {
        var command = new ToggleStatusCommand { PluginId = pluginId, ToggleStatus = ToggleStatus.Active };
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
    
    [HttpPost("disable/{pluginId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DisablePlugin([FromRoute] string pluginId)
    {
        var command = new ToggleStatusCommand { PluginId = pluginId, ToggleStatus = ToggleStatus.InActive };
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
}