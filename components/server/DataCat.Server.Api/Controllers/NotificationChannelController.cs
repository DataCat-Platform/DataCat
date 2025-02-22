namespace DataCat.Server.Api.Controllers;

public sealed class NotificationChannelController : ApiControllerBase
{
    [HttpGet("search")]
    [ProducesResponseType(typeof(NotificationChannelResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchNotificationChannels(
        [FromQuery] string? filter = null, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchNotificationChannelsQuery(page, pageSize, filter);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.Select(x => x.ToResponse()));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(NotificationChannelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetNotificationChannelById(Guid id)
    {
        var query = new GetNotificationChannelQuery(id);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.ToResponse());
    }
    
    [HttpPost("add")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddNotificationChannel(
        [FromBody] AddNotificationChannelRequest request)
    {
        var response = await SendAsync(request.ToAddCommand());
        return HandleCustomResponse(response);
    }
        
    [HttpPut("update/{notificationChannelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateNotificationChannel(
        [FromRoute] string notificationChannelId, 
        [FromBody] UpdateNotificationChannelRequest request)
    {
        var query = request.ToUpdateCommand(notificationChannelId);
        var response = await SendAsync(query);
        return HandleCustomResponse(response);
    }
    
    [HttpDelete("remove/{notificationChannelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveNotificationChannel(
        [FromRoute] string notificationChannelId)
    {
        var command = new RemoveNotificationCommand(notificationChannelId);
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
}