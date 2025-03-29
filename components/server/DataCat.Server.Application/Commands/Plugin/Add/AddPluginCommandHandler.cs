namespace DataCat.Server.Application.Commands.Plugin.Add;

public sealed class AddPluginCommandHandler(
    IRepository<PluginEntity, Guid> pluginRepository,
    IPluginStorage blobPlugin)
    : IRequestHandler<AddPluginCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddPluginCommand request, CancellationToken token)
    {
        var id = Guid.NewGuid();
        var pluginResult = CreatePluginEntity(id, request);

        if (pluginResult.IsFailure)
            return Result.Fail<Guid>(pluginResult.Errors!);
            
        var task1 = pluginRepository.AddAsync(pluginResult.Value, token);
        await using var stream = request.File.OpenReadStream();
        var task2 = blobPlugin.SavePlugin(id.ToString(), stream, token);

        await Task.WhenAll(task1, task2);
        
        return Result.Success(id);
    }

    private static Result<PluginEntity> CreatePluginEntity(Guid id, AddPluginCommand request)
    {
        var result = PluginEntity.Create(
            id,
            request.Name,
            request.Version,
            request.Description,
            request.Author,
            isEnabled: false,
            request.Settings,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow);
        
        return result;
    }
}