namespace DataCat.Server.Application.Queries.Plugins.Search;

public sealed class SearchPluginsQueryHandler(
    IPluginRepository pluginRepository)
    : IQueryHandler<SearchPluginsQuery, Page<SearchPluginsResponse>>
{
    public async Task<Result<Page<SearchPluginsResponse>>> Handle(SearchPluginsQuery request, CancellationToken token)
    {
        var result = await pluginRepository
            .SearchAsync(request.Filter, request.Page, request.PageSize, token);
        
        return Result.Success(result.ToResponsePage(SearchPluginsResponse.ToResponse));
    }
}