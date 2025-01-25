using DataCat.Server.Application.Commands.Plugin.ToggleStatus;

namespace DataCat.Server.Api.Controllers;

public sealed class PluginController : ApiControllerBase
{
    [HttpPost("add")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPlugin([FromForm] AddPluginRequest request)
    {
        var response = await SendAsync(request.ToAddCommand());
        return response.IsFailure 
            ? BadRequest(CreateProblemDetails(response.Errors)) 
            : Ok(response.Value);
    }
    
    [HttpDelete("remove/{pluginId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemovePlugin([FromRoute] string pluginId)
    {
        var command = new RemovePluginCommand(pluginId);
        var response = await SendAsync(command);
        return response.IsFailure 
            ? BadRequest(CreateProblemDetails(response.Errors))
            : Ok();
    }
    
    [HttpGet("list")]
    [ProducesResponseType(typeof(FullPluginResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlugins()
    {
        var query = new GetAllPluginsQuery();
        var response = await SendAsync(query);
        if (response.IsFailure)
            return BadRequest(CreateProblemDetails(response.Errors));

        var pluginsResponse = response.Value.Select(x => x.ToResponse());
        return Ok(pluginsResponse);
    }
    
    [HttpGet("{pluginId}")]
    [ProducesResponseType(typeof(FullPluginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlugin(string pluginId)
    {
        var query = new GetPluginQuery(pluginId);
        var response = await SendAsync(query);
        
        if (response.IsFailure)
            return BadRequest(CreateProblemDetails(response.Errors));

        var pluginResponse = response.Value.ToResponse();
        return Ok(pluginResponse);
    }
    
    [HttpPut("update/{pluginId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePluginConfig([FromRoute] string pluginId, [FromBody] UpdatePluginRequest request)
    {
        var query = request.ToUpdateCommand(pluginId);
        var response = await SendAsync(query);
        return response.IsFailure 
            ? BadRequest(CreateProblemDetails(response.Errors))
            : Ok();
    }
    
    [HttpPost("enable/{pluginId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EnablePlugin([FromRoute] string pluginId)
    {
        var command = new ToggleStatusCommand() { PluginId = pluginId, ToggleStatus = ToggleStatus.Active };
        var response = await SendAsync(command);
        return response.IsFailure 
            ? BadRequest(CreateProblemDetails(response.Errors))
            : Ok();
    }
    
    [HttpPost("disable/{pluginId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DisablePlugin([FromRoute] string pluginId)
    {
        var command = new ToggleStatusCommand() { PluginId = pluginId, ToggleStatus = ToggleStatus.InActive };
        var response = await SendAsync(command);
        return response.IsFailure 
            ? BadRequest(CreateProblemDetails(response.Errors))
            : Ok();
    }
}