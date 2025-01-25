namespace DataCat.Server.Application.Commands.Plugin.Update;

public sealed class UpdatePluginCommandHandler(
    IDefaultRepository<PluginEntity, Guid> pluginRepository)
    : IRequestHandler<UpdatePluginCommand, Result>
{
    public async Task<Result> Handle(UpdatePluginCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.PluginId);
        
        var plugin = await pluginRepository.GetByIdAsync(id, token);
        if (plugin is null)
            return Result.Fail(PluginError.NotFound(id.ToString()));
        
        plugin.LoadConfiguration(request.Description, request.Settings);
        await pluginRepository.UpdateAsync(plugin, token);
        
        return Result.Success();
    }
}