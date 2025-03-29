namespace DataCat.Server.Application.Commands.NotificationChannel.Add;

public sealed class AddNotificationCommandHandler(
    IRepository<NotificationChannelEntity, Guid> notificationChannelRepository,
    NotificationChannelManager notificationChannelManager)
    : IRequestHandler<AddNotificationCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
    {
        var destinationType = NotificationDestination.FromValue(request.DestinationType);
        if (destinationType is null)
            return Result.Fail<Guid>(NotificationChannelError.DestinationNotSupported);

        var factory = notificationChannelManager.GetNotificationChannelFactory(destinationType);
        var settingsResult = factory.Create(request.Settings);
        if (settingsResult.IsFailure)
            return Result.Fail<Guid>(settingsResult.Errors!);

        var notificationResult = NotificationChannelEntity.Create(
            Guid.NewGuid(),
            destinationType,
            settingsResult.Value);
        
        if (notificationResult.IsFailure)
            return Result.Fail<Guid>(notificationResult.Errors!);
        
        await notificationChannelRepository.AddAsync(notificationResult.Value, cancellationToken);
        return Result.Success(notificationResult.Value.Id);
    }
}