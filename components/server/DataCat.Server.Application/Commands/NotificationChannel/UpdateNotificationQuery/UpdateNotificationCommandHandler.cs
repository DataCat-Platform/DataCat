namespace DataCat.Server.Application.Commands.NotificationChannel.UpdateNotificationQuery;

public sealed class UpdateNotificationCommandHandler(
    IDefaultRepository<NotificationChannelEntity, Guid> notificationRepository)
    : IRequestHandler<UpdateNotificationCommand, Result>
{
    public async Task<Result> Handle(UpdateNotificationCommand request, CancellationToken token)
    {
        var notification = await notificationRepository.GetByIdAsync(Guid.Parse(request.NotificationChannelId), token);
        if (notification is null)
            return Result.Fail(NotificationChannelError.NotFound(request.NotificationChannelId));

        var destinationType = NotificationDestination.FromValue(request.DestinationType);
        if (destinationType is null)
            return Result.Fail(NotificationChannelError.DestinationNotSupported);
        
        notification.ChangeConfiguration(destinationType, request.Settings);
        
        await notificationRepository.UpdateAsync(notification, token);
        return Result.Success();
    }
}