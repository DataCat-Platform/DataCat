namespace DataCat.Server.Api.Controllers;

public sealed class PanelController : ApiControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OverviewDashboardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPanelById(Guid id)
    {
        var query = new GetPanelQuery(id);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.ToResponse());
    }
    
    [HttpPost("add")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPanel(
        [FromBody] AddPanelRequest request)
    {
        var response = await SendAsync(request.ToAddCommand());
        return HandleCustomResponse(response);
    }
        
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePanel(
        [FromBody] UpdatePanelRequest request)
    {
        var query = request.ToUpdateCommand();
        var response = await SendAsync(query);
        return HandleCustomResponse(response);
    }
    
    [HttpDelete("remove/{panelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemovePanel(
        [FromRoute] string panelId)
    {
        var command = new RemovePanelCommand(panelId);
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
}