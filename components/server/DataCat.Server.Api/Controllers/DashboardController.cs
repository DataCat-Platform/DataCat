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
        if (response.IsFailure)
            return BadRequest(CreateProblemDetails(response.Errors));

        var pluginsResponse = response.Value.Select(x => x.ToResponse());
        return Ok(pluginsResponse);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OverviewDashboardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDashboardById(Guid id)
    {
        var query = new GetDashboardQuery(id);
        var response = await SendAsync(query);
        
        if (response.IsFailure)
            return BadRequest(CreateProblemDetails(response.Errors));

        var pluginResponse = response.Value.ToResponse();
        return Ok(pluginResponse);
    }
}