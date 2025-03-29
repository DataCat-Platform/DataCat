namespace DataCat.Server.Application.Queries.NotificationChannel.Get;

public class GetNotificationChannelQueryHandler(
    IRepository<NotificationChannelEntity, Guid> notificationChannelRepository)
    : IRequestHandler<GetNotificationChannelQuery, Result<GetNotificationChannelResponse>>
{
    public async Task<Result<GetNotificationChannelResponse>> Handle(GetNotificationChannelQuery request, CancellationToken token)
    {
        var entity = await notificationChannelRepository.GetByIdAsync(request.NotificationChannelId, token);
        return entity is null 
            ? Result.Fail<GetNotificationChannelResponse>(NotificationChannelError.NotFound(request.NotificationChannelId.ToString())) 
            : Result<GetNotificationChannelResponse>.Success(entity.ToResponse());
    }
}