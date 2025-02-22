namespace DataCat.Server.Application.Commands.Alert.UpdateAlertQuery;

public sealed class UpdateAlertQueryCommandHandler(
    IDefaultRepository<AlertEntity, Guid> alertRepository,
    IDefaultRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<UpdateAlertQueryCommand, Result>
{
    public async Task<Result> Handle(UpdateAlertQueryCommand request, CancellationToken cancellationToken)
    {
        var alert = await alertRepository.GetByIdAsync(Guid.Parse(request.AlertId), cancellationToken);
        if (alert is null)
            return Result.Fail(AlertError.NotFound(request.AlertId));
        
        var dataSource = await dataSourceRepository.GetByIdAsync(Guid.Parse(request.DataSourceId), cancellationToken);
        if (dataSource is null)
            return Result.Fail<Guid>(DataSourceError.NotFound(request.DataSourceId));

        var queryResult = QueryEntity.Create(dataSource, request.RawQuery);
        if (queryResult.IsFailure)
            return Result.Fail<Guid>(queryResult.Errors!);
        
        alert.ChangeAlertQuery(queryResult.Value);
        alert.ChangeDescription(request.Description);
        
        await alertRepository.UpdateAsync(alert, cancellationToken);
        return Result.Success();
    }
}