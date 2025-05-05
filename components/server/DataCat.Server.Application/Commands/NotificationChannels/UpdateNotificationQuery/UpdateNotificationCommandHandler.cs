namespace DataCat.Server.Application.Commands.NotificationChannels.UpdateNotificationQuery;

public sealed class UpdateNotificationCommandHandler(
    IRepository<NotificationChannel, Guid> notificationBaseRepository,
    INotificationDestinationRepository notificationDestinationRepository,
    INotificationChannelRepository notificationRepository,
    NotificationChannelManager notificationChannelManager)
    : ICommandHandler<UpdateNotificationCommand>
{
    public async Task<Result> Handle(UpdateNotificationCommand request, CancellationToken token)
    {
        var notification = await notificationBaseRepository.GetByIdAsync(Guid.Parse(request.NotificationChannelId), token);
        if (notification is null)
            return Result.Fail(NotificationChannelError.NotFound(request.NotificationChannelId));

        var destination = await notificationDestinationRepository.GetByNameAsync(request.DestinationTypeName, token);
        if (destination is null)
            return Result.Fail(NotificationChannelError.DestinationNotSupported);
        
        var factory = notificationChannelManager.GetNotificationChannelFactory(destination);
        var settingsResult = factory.Create(destination, request.Settings);
        if (settingsResult.IsFailure)
            return Result.Fail<Guid>(settingsResult.Errors!);
        
        notification.ChangeConfiguration(settingsResult.Value);
        
        await notificationRepository.UpdateAsync(notification, token);
        return Result.Success();
    }
}