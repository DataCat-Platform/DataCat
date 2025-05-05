namespace DataCat.Server.Application.Commands.NotificationChannels.Remove;

public sealed class RemoveNotificationCommandHandler(
    INotificationChannelRepository notificationRepository)
    : ICommandHandler<RemoveNotificationCommand>
{
    public async Task<Result> Handle(RemoveNotificationCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.NotificationId);
        await notificationRepository.DeleteAsync(id, cancellationToken);
        return Result.Success();
    }
}