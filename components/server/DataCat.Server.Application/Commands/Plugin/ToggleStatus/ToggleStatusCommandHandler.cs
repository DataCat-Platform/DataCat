namespace DataCat.Server.Application.Commands.Plugin.ToggleStatus;

public class ToggleStatusCommandHandler(
    ITogglePluginStatusRepository togglePluginStatusRepository)
    : IRequestHandler<ToggleStatusCommand, Result>
{
    public async Task<Result> Handle(ToggleStatusCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.PluginId);
        
        var isEnabled = request.ToggleStatus == ToggleStatus.Active;
        var isSuccess = await togglePluginStatusRepository.ToggleStatusAsync(id, isEnabled, token);
        
        // todo: add loading plugin (for example: Alerts)
        
        return isSuccess ? Result.Success() : Result.Fail(PluginError.ErrorDuringUpdate);
    }
}