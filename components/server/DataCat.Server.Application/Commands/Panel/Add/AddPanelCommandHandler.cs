namespace DataCat.Server.Application.Commands.Panel.Add;

public sealed class AddPanelCommandHandler(
    IDefaultRepository<PanelEntity, Guid> panelRepository,
    IDefaultRepository<DataSourceEntity, Guid> dataSourceRepository,
    IDefaultRepository<DashboardEntity, Guid> dashboardRepository)
    : IRequestHandler<AddPanelCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddPanelCommand request, CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByIdAsync(Guid.Parse(request.DataSourceId), cancellationToken);
        if (dataSource is null)
        {
            return Result.Fail<Guid>(DataSourceError.NotFound(request.DataSourceId));
        }
        var isDashboardExist = await dashboardRepository.GetByIdAsync(Guid.Parse(request.DashboardId), cancellationToken) is not null;
        if (!isDashboardExist)
        {
            return Result.Fail<Guid>(DashboardError.NotFound(request.DashboardId));
        }
        
        var panelId = Guid.NewGuid();
        var result = CreatePanel(panelId, request, dataSource);

        if (result.IsFailure)
            return Result.Fail<Guid>(result.Errors!);
            
        await panelRepository.AddAsync(result.Value, cancellationToken);
        return Result.Success(panelId);
    }
    
    private static Result<PanelEntity> CreatePanel(Guid id, AddPanelCommand request, DataSourceEntity dataSource)
    {
        var panelType = PanelType.FromValue(request.Type);
        if (panelType is null)
            return Result.Fail<PanelEntity>(PanelError.InvalidPanelType);

        var queryResult = QueryEntity.Create(dataSource, request.RawQuery);
        if (queryResult.IsFailure)
            return Result.Fail<PanelEntity>(queryResult.Errors!);
        
        var layoutResult = DataCatLayout.Create(request.PanelX, request.PanelY, request.Width, request.Height);
        if (layoutResult.IsFailure)
            return Result.Fail<PanelEntity>(layoutResult.Errors!);

        return PanelEntity.Create(id,
            request.Title,
            panelType,
            queryResult.Value,
            layoutResult.Value,
            Guid.Parse(request.DashboardId));
    }
}