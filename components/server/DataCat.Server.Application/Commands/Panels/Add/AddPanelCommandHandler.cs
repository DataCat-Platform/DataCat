namespace DataCat.Server.Application.Commands.Panels.Add;

public sealed class AddPanelCommandHandler(
    IRepository<Panel, Guid> panelRepository,
    IRepository<DataSource, Guid> dataSourceRepository,
    IRepository<Dashboard, Guid> dashboardRepository,
    NamespaceContext namespaceContext)
    : ICommandHandler<AddPanelCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddPanelCommand request, CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByIdAsync(Guid.Parse(request.DataSourceId), cancellationToken);
        if (dataSource is null)
        {
            return Result.Fail<Guid>(DataSourceError.NotFoundById(request.DataSourceId));
        }
        var isDashboardExist = await dashboardRepository.GetByIdAsync(Guid.Parse(request.DashboardId), cancellationToken) is not null;
        if (!isDashboardExist)
        {
            return Result.Fail<Guid>(DashboardError.NotFound(request.DashboardId));
        }
        
        var panelId = Guid.NewGuid();
        var result = CreatePanel(panelId, request, dataSource, namespaceContext.GetNamespaceId());

        if (result.IsFailure)
            return Result.Fail<Guid>(result.Errors!);
            
        await panelRepository.AddAsync(result.Value, cancellationToken);
        return Result.Success(panelId);
    }
    
    private static Result<Panel> CreatePanel(Guid id, AddPanelCommand request, DataSource dataSource, Guid namespaceId)
    {
        var panelType = PanelType.FromValue(request.Type);
        if (panelType is null)
            return Result.Fail<Panel>(PanelError.InvalidPanelType);

        var queryResult = Query.Create(dataSource, request.RawQuery);
        if (queryResult.IsFailure)
            return Result.Fail<Panel>(queryResult.Errors!);
        
        return Panel.Create(id,
            request.Title,
            panelType,
            queryResult.Value,
            new DataCatLayout(request.Layout),
            Guid.Parse(request.DashboardId),
            request.StyleConfiguration,
            namespaceId);
    }
}