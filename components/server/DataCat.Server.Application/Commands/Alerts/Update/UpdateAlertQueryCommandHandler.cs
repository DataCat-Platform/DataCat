namespace DataCat.Server.Application.Commands.Alerts.Update;

public sealed class UpdateAlertQueryCommandHandler(
    IRepository<Alert, Guid> alertBaseRepository,
    IAlertRepository alertRepository,
    IRepository<DataSource, Guid> dataSourceRepository)
    : ICommandHandler<UpdateAlertQueryCommand>
{
    public async Task<Result> Handle(UpdateAlertQueryCommand request, CancellationToken cancellationToken)
    {
        var alert = await alertBaseRepository.GetByIdAsync(Guid.Parse(request.AlertId), cancellationToken);
        if (alert is null)
            return Result.Fail(AlertError.NotFound(request.AlertId));
        
        var dataSource = await dataSourceRepository.GetByIdAsync(Guid.Parse(request.DataSourceId), cancellationToken);
        if (dataSource is null)
            return Result.Fail<Guid>(DataSourceError.NotFoundById(request.DataSourceId));

        var queryResult = Query.Create(dataSource, request.RawQuery);
        if (queryResult.IsFailure)
            return Result.Fail<Guid>(queryResult.Errors!);
        
        alert.ChangeAlertQuery(queryResult.Value);
        alert.ChangeDescription(request.Description);
        alert.ChangeTemplate(request.Template);
        
        await alertRepository.UpdateAsync(alert, cancellationToken);
        return Result.Success();
    }
}