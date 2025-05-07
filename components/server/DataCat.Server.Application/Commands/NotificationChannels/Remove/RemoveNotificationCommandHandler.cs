namespace DataCat.Server.Application.Commands.NotificationChannels.Remove;

public sealed class RemoveNotificationCommandHandler(
    INotificationChannelRepository notificationRepository)
    : ICommandHandler<RemoveNotificationCommand>
{
    public async Task<Result> Handle(RemoveNotificationCommand request, CancellationToken cancellationToken)
    {
        await notificationRepository.DeleteAsync(request.NotificationId, cancellationToken);
        return Result.Success();
    }
}