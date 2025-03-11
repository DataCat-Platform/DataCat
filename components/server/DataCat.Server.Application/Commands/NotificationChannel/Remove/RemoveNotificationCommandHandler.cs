namespace DataCat.Server.Application.Commands.NotificationChannel.Remove;

public sealed class RemoveNotificationCommandHandler(
    IDefaultRepository<NotificationChannelEntity, Guid> notificationRepository)
    : IRequestHandler<RemoveNotificationCommand, Result>
{
    public async Task<Result> Handle(RemoveNotificationCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.NotificationId);
        await notificationRepository.DeleteAsync(id, cancellationToken);
        return Result.Success();
    }
}