namespace DataCat.Server.Application.Queries.Plugin.GetAll;

public sealed class GetAllPluginsQueryHandler(IDefaultRepository<PluginEntity, Guid> pluginRepository)
    : IRequestHandler<GetAllPluginsQuery, Result<IEnumerable<PluginEntity>>>
{
    public async Task<Result<IEnumerable<PluginEntity>>> Handle(GetAllPluginsQuery request, CancellationToken token)
    {
        // todo: maybe add without result pattern to improve performance
        var result = await pluginRepository.GetAllAsync(token);
        return Result.Success(result);
    }
}