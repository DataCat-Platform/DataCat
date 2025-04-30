namespace DataCat.Server.Application.Commands.Plugins.Update;

public sealed class UpdatePluginCommandHandler(
    IRepository<Plugin, Guid> pluginBaseRepository,
    IPluginRepository pluginRepository)
    : IRequestHandler<UpdatePluginCommand, Result>
{
    public async Task<Result> Handle(UpdatePluginCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.PluginId);
        
        var plugin = await pluginBaseRepository.GetByIdAsync(id, token);
        if (plugin is null)
            return Result.Fail(PluginError.NotFound(id.ToString()));
        
        plugin.LoadConfiguration(request.Description, request.Settings);
        await pluginRepository.UpdateAsync(plugin, token);
        
        return Result.Success();
    }
}