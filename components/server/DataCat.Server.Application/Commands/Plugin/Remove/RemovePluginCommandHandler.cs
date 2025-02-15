namespace DataCat.Server.Application.Commands.Plugin.Remove;

public sealed class RemovePluginCommandHandler(
    IDefaultRepository<PluginEntity, Guid> pluginRepository)
    : IRequestHandler<RemovePluginCommand, Result>
{
    public async Task<Result> Handle(RemovePluginCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.PluginId);

        await pluginRepository.DeleteAsync(id, token);
        // todo: add background thread which clears unused plugins
        return Result.Success();
    }
}