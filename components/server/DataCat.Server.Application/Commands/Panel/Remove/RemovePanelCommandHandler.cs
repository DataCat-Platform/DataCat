namespace DataCat.Server.Application.Commands.Panel.Remove;

public sealed class RemovePanelCommandHandler(
    IDefaultRepository<PanelEntity, Guid> panelRepository)
    : IRequestHandler<RemovePanelCommand, Result>
{
    public async Task<Result> Handle(RemovePanelCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.PanelId);

        await panelRepository.DeleteAsync(id, token);
        return Result.Success();
    }
}