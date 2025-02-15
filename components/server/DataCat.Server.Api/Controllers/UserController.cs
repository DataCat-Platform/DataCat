namespace DataCat.Server.Api.Controllers;

public class UserController : ApiControllerBase
{
    [HttpGet("search")]
    [ProducesResponseType(typeof(UserResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchUsers(
        [FromQuery] string? filter = null, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchUsersQuery(page, pageSize, filter);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.Select(x => x.ToResponse()));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var query = new GetUserQuery(id);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.ToResponse());
    }
    
    [HttpPost("add")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUser(
        [FromBody] AddUserRequest request)
    {
        var response = await SendAsync(request.ToAddCommand());
        return HandleCustomResponse(response);
    }
        
    [HttpPut("update-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserRoleRequest request)
    {
        var query = request.ToUpdateRoleCommand();
        var response = await SendAsync(query);
        return HandleCustomResponse(response);
    }
    
    [HttpPut("update-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserInfoRequest request)
    {
        var query = request.ToUpdateSettingsCommand();
        var response = await SendAsync(query);
        return HandleCustomResponse(response);
    }
    
    [HttpDelete("remove/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveDashboard(
        [FromRoute] string userId)
    {
        var command = new RemoveUserCommand(userId);
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
}