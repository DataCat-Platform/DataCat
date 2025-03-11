namespace DataCat.Server.Api.Controllers;

public sealed class DataSourceController : ApiControllerBase
{
    [HttpPost("add")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDataSource([FromBody] AddDataSource request)
    {
        var response = await SendAsync(request.ToAddCommand());
        return HandleCustomResponse(response);
    }
    
    [HttpDelete("remove/{dataSourceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveDataSource([FromRoute] string dataSourceId)
    {
        var command = new RemoveDataSourceCommand(dataSourceId);
        var response = await SendAsync(command);
        return HandleCustomResponse(response);
    }
    
    [HttpGet("search")]
    [ProducesResponseType(typeof(DataSourceResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPlugins(
        [FromQuery] string? filter = null, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new SearchDataSourcesQuery(page, pageSize, filter);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.Select(x => x.ToResponse()));
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FullPluginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDataSource([FromRoute] Guid id)
    {
        var query = new GetDataSourceQuery(id);
        var response = await SendAsync(query);
        return HandleCustomResponse(response,
            map: result => result.Value.ToResponse());
    }
    
    [HttpPut("update/{dataSourceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePluginConfig([FromRoute] string dataSourceId, [FromBody] UpdateDataSource request)
    {
        var query = request.ToUpdateCommand(dataSourceId);
        var response = await SendAsync(query);
        return HandleCustomResponse(response);
    }
}