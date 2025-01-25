namespace DataCat.Server.Application.Queries.Plugin.Get;

public sealed record GetPluginQueryHandler(
    IDefaultRepository<PluginEntity, Guid> pluginRepository)
    : IRequestHandler<GetPluginQuery, Result<PluginEntity>>
{
    public async Task<Result<PluginEntity>> Handle(GetPluginQuery request, CancellationToken token)
    {
        var id = Guid.Parse(request.PluginId);
        
        var entity = await pluginRepository.GetByIdAsync(id, token);
        return entity is null 
            ? Result.Fail<PluginEntity>(PluginError.NotFound(id.ToString())) 
            : Result<PluginEntity>.Success(entity);
    }
}