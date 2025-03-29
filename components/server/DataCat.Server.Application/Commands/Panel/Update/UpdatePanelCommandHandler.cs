namespace DataCat.Server.Application.Commands.Panel.Update;

public sealed class UpdatePanelCommandHandler(
    IRepository<PanelEntity, Guid> panelBaseRepository,
    IPanelRepository panelRepository,
    IRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<UpdatePanelCommand, Result>
{
    public async Task<Result> Handle(UpdatePanelCommand request, CancellationToken cancellationToken)
    {
        var panelResult = await GetEntityAsync(panelBaseRepository, request.PanelId, PanelError.NotFound(request.PanelId));
        if (panelResult.IsFailure) 
            return panelResult;

        var dataSourceResult = await GetEntityAsync(dataSourceRepository, request.DataSourceId, DataSourceError.NotFound(request.DataSourceId));
        if (dataSourceResult.IsFailure) 
            return dataSourceResult;
        
        var panel = panelResult.Value;
        var dataSource = dataSourceResult.Value;

        panel.UpdateTitle(request.Title);

        var panelRawQueryResult = CreateQueryAndPanelType(request, dataSource);
        if (panelRawQueryResult.IsFailure) 
            return panelRawQueryResult;

        var (newQuery, newPanel) = panelRawQueryResult.Value;
        panel.UpdatePanelType(newPanel, newQuery);

        var layoutResult = CreateLayout(request);
        if (layoutResult.IsFailure) 
            return layoutResult;

        panel.UpdateLayout(layoutResult.Value);

        await panelRepository.UpdateAsync(panel, cancellationToken);
        return Result.Success();
    }

    private static async Task<Result<T>> GetEntityAsync<T>(
        IRepository<T, Guid> repository,
        string id,
        BaseError error)
        where T : class
    {
        if (!Guid.TryParse(id, out var guid))
            return Result.Fail<T>(error);

        var entity = await repository.GetByIdAsync(guid);
        return entity is not null ? Result.Success(entity) : Result.Fail<T>(error);
    }

    private static Result<(QueryEntity, PanelType)> CreateQueryAndPanelType(UpdatePanelCommand request, DataSourceEntity dataSource)
    {
        var queryResult = QueryEntity.Create(dataSource, request.RawQuery);
        if (queryResult.IsFailure) 
            return Result.Fail<(QueryEntity, PanelType)>(queryResult.Errors!);

        var panelResult = PanelType.FromValue(request.Type);
        return panelResult is not null
            ? Result.Success((queryResult.Value, panelResult))
            : Result.Fail<(QueryEntity, PanelType)>(PanelError.InvalidPanelType);
    }

    private static Result<DataCatLayout> CreateLayout(UpdatePanelCommand request) =>
        DataCatLayout.Create(request.PanelX, request.PanelY, request.Width, request.Height);
}