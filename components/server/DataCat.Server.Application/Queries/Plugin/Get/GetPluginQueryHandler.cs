namespace DataCat.Server.Application.Queries.Plugin.Get;

public sealed record GetPluginQueryHandler(
    IDefaultRepository<PluginEntity, Guid> pluginRepository)
    : IRequestHandler<GetPluginQuery, Result<PluginEntity>>
{
    public async Task<Result<PluginEntity>> Handle(GetPluginQuery request, CancellationToken token)
    {
        var entity = await pluginRepository.GetByIdAsync(request.PluginId, token);
        return entity is null 
            ? Result.Fail<PluginEntity>(PluginError.NotFound(request.PluginId.ToString())) 
            : Result<PluginEntity>.Success(entity);
    }
}