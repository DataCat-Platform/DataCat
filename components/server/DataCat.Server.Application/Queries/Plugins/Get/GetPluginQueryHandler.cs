namespace DataCat.Server.Application.Queries.Plugins.Get;

public sealed record GetPluginQueryHandler(
    IRepository<Plugin, Guid> pluginRepository)
    : IRequestHandler<GetPluginQuery, Result<GetPluginResponse>>
{
    public async Task<Result<GetPluginResponse>> Handle(GetPluginQuery request, CancellationToken token)
    {
        var entity = await pluginRepository.GetByIdAsync(request.PluginId, token);
        return entity is null 
            ? Result.Fail<GetPluginResponse>(PluginError.NotFound(request.PluginId.ToString())) 
            : Result<GetPluginResponse>.Success(entity.ToResponse());
    }
}