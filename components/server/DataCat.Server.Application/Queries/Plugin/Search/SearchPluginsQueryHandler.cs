namespace DataCat.Server.Application.Queries.Plugin.Search;

public sealed class SearchPluginsQueryHandler(IDefaultRepository<PluginEntity, Guid> pluginRepository)
    : IRequestHandler<SearchPluginsQuery, Result<IEnumerable<PluginEntity>>>
{
    public async Task<Result<IEnumerable<PluginEntity>>> Handle(SearchPluginsQuery request, CancellationToken token)
    {
        var result = await pluginRepository.SearchAsync(request.Filter, request.Page, request.PageSize, token);
        return Result.Success(result);
    }
}