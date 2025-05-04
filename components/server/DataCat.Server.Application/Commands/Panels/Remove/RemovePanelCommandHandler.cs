namespace DataCat.Server.Application.Commands.Panels.Remove;

public sealed class RemovePanelCommandHandler(
    IPanelRepository panelRepository)
    : ICommandHandler<RemovePanelCommand>
{
    public async Task<Result> Handle(RemovePanelCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.PanelId);

        await panelRepository.DeleteAsync(id, token);
        return Result.Success();
    }
}