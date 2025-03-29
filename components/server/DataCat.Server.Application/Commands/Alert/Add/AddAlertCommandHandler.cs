namespace DataCat.Server.Application.Commands.Alert.Add;

public sealed class AddAlertCommandHandler(
    IRepository<AlertEntity, Guid> alertRepository,
    IRepository<DataSourceEntity, Guid> dataSourceRepository,
    IRepository<NotificationChannelEntity, Guid> notificationChannelRepository)
    : IRequestHandler<AddAlertCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddAlertCommand request, CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByIdAsync(Guid.Parse(request.DataSourceId), cancellationToken);
        if (dataSource is null)
            return Result.Fail<Guid>(DataSourceError.NotFound(request.DataSourceId));

        var queryResult = QueryEntity.Create(dataSource, request.RawQuery);
        if (queryResult.IsFailure)
            return Result.Fail<Guid>(queryResult.Errors!);
        
        var notificationChannel = await notificationChannelRepository.GetByIdAsync(Guid.Parse(request.NotificationChannelId), cancellationToken);
        if (notificationChannel is null)
            return Result.Fail<Guid>(NotificationChannelError.NotFound(request.NotificationChannelId));

        var alertResult = AlertEntity.Create(
            Guid.NewGuid(),
            request.Description,
            queryResult.Value,
            AlertStatus.InActive,
            notificationChannel,
            previousExecution: DateTimeUtc.Init(),
            nextExecution: DateTime.UtcNow.Add(request.RepeatInterval),
            request.WaitTimeBeforeAlerting,
            request.RepeatInterval);
        
        if (alertResult.IsFailure)
            return Result.Fail<Guid>(alertResult.Errors!);
        
        await alertRepository.AddAsync(alertResult.Value, cancellationToken);
        return Result.Success(alertResult.Value.Id);
    }
}