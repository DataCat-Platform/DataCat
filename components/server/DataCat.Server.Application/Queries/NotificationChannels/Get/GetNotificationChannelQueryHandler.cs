namespace DataCat.Server.Application.Queries.NotificationChannels.Get;

public class GetNotificationChannelQueryHandler(
    IRepository<NotificationChannel, Guid> notificationChannelRepository)
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