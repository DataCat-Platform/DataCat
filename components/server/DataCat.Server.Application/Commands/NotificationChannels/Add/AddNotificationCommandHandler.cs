namespace DataCat.Server.Application.Commands.NotificationChannels.Add;

public sealed class AddNotificationCommandHandler(
    INotificationDestinationRepository notificationDestinationRepository,
    IRepository<NotificationChannel, Guid> notificationChannelRepository,
    NotificationChannelManager notificationChannelManager)
    : IRequestHandler<AddNotificationCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
    {
        var destination = await notificationDestinationRepository.GetByNameAsync(request.DestinationTypeName, cancellationToken);
        if (destination is null)
            return Result.Fail<Guid>(NotificationChannelError.DestinationNotSupported);

        var factory = notificationChannelManager.GetNotificationChannelFactory(destination);
        
        var settingsResult = factory.Create(destination, request.Settings);
        if (settingsResult.IsFailure)
            return Result.Fail<Guid>(settingsResult.Errors!);

        var notificationResult = NotificationChannel.Create(
            Guid.NewGuid(),
            settingsResult.Value);
        
        if (notificationResult.IsFailure)
            return Result.Fail<Guid>(notificationResult.Errors!);
        
        await notificationChannelRepository.AddAsync(notificationResult.Value, cancellationToken);
        return Result.Success(notificationResult.Value.Id);
    }
}