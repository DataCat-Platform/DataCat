namespace DataCat.Server.Application.Queries.Panel.Get;

public class GetPanelQueryHandler(
    IDefaultRepository<PanelEntity, Guid> panelRepository)
    : IRequestHandler<GetPanelQuery, Result<PanelEntity>>
{
    public async Task<Result<PanelEntity>> Handle(GetPanelQuery request, CancellationToken token)
    {
        var entity = await panelRepository.GetByIdAsync(request.PanelId, token);
        return entity is null 
            ? Result.Fail<PanelEntity>(PanelError.NotFound(request.PanelId.ToString())) 
            : Result<PanelEntity>.Success(entity);
    }
}