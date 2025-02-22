namespace DataCat.Server.Application.Queries.NotificationChannel.Get;

public class GetNotificationChannelQueryHandler(
    IDefaultRepository<NotificationChannelEntity, Guid> notificationChannelRepository)
    : IRequestHandler<GetNotificationChannelQuery, Result<NotificationChannelEntity>>
{
    public async Task<Result<NotificationChannelEntity>> Handle(GetNotificationChannelQuery request, CancellationToken token)
    {
        var entity = await notificationChannelRepository.GetByIdAsync(request.NotificationChannelId, token);
        return entity is null 
            ? Result.Fail<NotificationChannelEntity>(NotificationChannelError.NotFound(request.NotificationChannelId.ToString())) 
            : Result<NotificationChannelEntity>.Success(entity);
    }
}