namespace DataCat.Server.Api.Controllers;

public class AlertController : ApiControllerBase
{
    [HttpGet("search")]
    [ProducesResponseType(typeof(AlertResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAlerts(
        [FromQuery] string? filter = null, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchAlertsQuery(page, pageSize, filter);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.Select(x => x.ToResponse()));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AlertResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAlertById(Guid id)
    {
        var query = new GetAlertQuery(id);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.ToResponse());
    }
    
    [HttpPost("add")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAlert(
        [FromBody] AddAlertRequest request)
    {
        var response = await SendAsync(request.ToAddCommand());
        return HandleCustomResponse(response);
    }
        
    [HttpPut("update/{alertId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAlert(
        [FromRoute] string alertId, 
        [FromBody] UpdateAlertRequest request)
    {
        var query = request.ToUpdateCommand(alertId);
        var response = await SendAsync(query);
        return HandleCustomResponse(response);
    }
    
    [HttpDelete("remove/{alertId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveAlert(
        [FromRoute] string alertId)
    {
        var command = new RemoveAlertCommand(alertId);
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
    
    [HttpPost("mute/{alertId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MuteAlert(
        [FromRoute] string alertId,
        [FromQuery] TimeSpan nextExecutionTime)
    {
        var command = new MuteAlertCommand(alertId, nextExecutionTime);
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
}