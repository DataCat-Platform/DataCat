namespace DataCat.Server.Application.Commands.Plugins.Remove;

public sealed class RemovePluginCommandHandler(
    IPluginRepository pluginRepository)
    : ICommand<RemovePluginCommand>
{
    public async Task<Result> Handle(RemovePluginCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.PluginId);

        await pluginRepository.DeleteAsync(id, token);
        // todo: add background thread which clears unused plugins
        return Result.Success();
    }
}