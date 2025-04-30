namespace DataCat.Server.Application.Queries.Panels.Get;

public class GetPanelQueryHandler(
    IRepository<Panel, Guid> panelRepository)
    : IRequestHandler<GetPanelQuery, Result<GetPanelResponse>>
{
    public async Task<Result<GetPanelResponse>> Handle(GetPanelQuery request, CancellationToken token)
    {
        var entity = await panelRepository.GetByIdAsync(request.PanelId, token);
        return entity is null 
            ? Result.Fail<GetPanelResponse>(PanelError.NotFound(request.PanelId.ToString())) 
            : Result<GetPanelResponse>.Success(entity.ToResponse());
    }
}