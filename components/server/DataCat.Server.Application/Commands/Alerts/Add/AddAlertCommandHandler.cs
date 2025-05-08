namespace DataCat.Server.Application.Commands.Alerts.Add;

public sealed class AddAlertCommandHandler(
    IRepository<Alert, Guid> alertRepository,
    IRepository<DataSource, Guid> dataSourceRepository,
    INotificationChannelGroupRepository notificationChannelGroupRepository)
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
        
        var notificationChannelGroup = await notificationChannelGroupRepository.GetByName(request.NotificationChannelGroupName, cancellationToken);
        if (notificationChannelGroup is null)
            return Result.Fail<Guid>(NotificationChannelGroupError.NotFound(request.NotificationChannelGroupName));

        var alertResult = Alert.Create(
            id: Guid.NewGuid(),
            request.Description,
            request.Template,
            queryResult.Value,
            AlertStatus.Ok,
            notificationChannelGroup,
            previousExecution: DateTimeUtc.Init(),
            nextExecution: DateTime.UtcNow.Add(request.RepeatInterval),
            request.WaitTimeBeforeAlerting,
            request.RepeatInterval,
            request.Tags.Select(x => new Tag(x)).ToList());
        
        if (alertResult.IsFailure)
            return Result.Fail<Guid>(alertResult.Errors!);
        
        await alertRepository.AddAsync(alertResult.Value, cancellationToken);
        return Result.Success(alertResult.Value.Id);
    }
}