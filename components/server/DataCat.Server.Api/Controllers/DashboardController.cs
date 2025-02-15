namespace DataCat.Server.Api.Controllers;

public sealed class DashboardController : ApiControllerBase
{
    [HttpGet("search")]
    [ProducesResponseType(typeof(HomeSearchDashboardResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchDashboards(
        [FromQuery] string? filter = null, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchDashboardsQuery(page, pageSize, filter);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.Select(x => x.ToResponse()));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OverviewDashboardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDashboardById(Guid id)
    {
        var query = new GetDashboardQuery(id);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.ToResponse());
    }
    
    [HttpGet("{id:guid}/full")]
    [ProducesResponseType(typeof(OverviewDashboardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFullDashboardById(Guid id)
    {
        var query = new GetDashboardQuery(id);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.ToFullResponse());
    }
    
    [HttpPost("add")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDashboard(
        [FromBody] AddDashboardRequest request)
    {
        var response = await SendAsync(request.ToAddCommand());
        return HandleCustomResponse(response);
    }
        
    [HttpPut("update/{dashboardId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDashboard(
        [FromRoute] string dashboardId, 
        [FromBody] UpdateDashboardRequest request)
    {
        var query = request.ToUpdateCommand(dashboardId);
        var response = await SendAsync(query);
        return HandleCustomResponse(response);
    }
    
    [HttpDelete("remove/{dashboardId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveDashboard(
        [FromRoute] string dashboardId)
    {
        var command = new RemoveDashboardCommand(dashboardId);
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
    
    [HttpPost("add-user")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDashboard(
        [FromBody] AddUserToDashboardRequest request)
    {
        var response = await SendAsync(request.ToCommand());
        return HandleCustomResponse(response);
    }
}