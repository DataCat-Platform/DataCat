namespace DataCat.Server.Application.Commands.Alerts.Add;

public sealed class AddAlertCommandHandler(
    IRepository<Alert, Guid> alertRepository,
    IRepository<DataSource, Guid> dataSourceRepository,
    IRepository<NotificationChannel, Guid> notificationChannelRepository)
    : ICommandHandler<AddAlertCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddAlertCommand request, CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByIdAsync(Guid.Parse(request.DataSourceId), cancellationToken);
        if (dataSource is null)
            return Result.Fail<Guid>(DataSourceError.NotFoundById(request.DataSourceId));

        var queryResult = Query.Create(dataSource, request.RawQuery);
        if (queryResult.IsFailure)
            return Result.Fail<Guid>(queryResult.Errors!);
        
        var notificationChannel = await notificationChannelRepository.GetByIdAsync(Guid.Parse(request.NotificationChannelId), cancellationToken);
        if (notificationChannel is null)
            return Result.Fail<Guid>(NotificationChannelError.NotFound(request.NotificationChannelId));

        var alertResult = Alert.Create(
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