namespace DataCat.Server.Application.Queries.NotificationChannels.Get;

public sealed class GetNotificationChannelQueryHandler(
    IRepository<NotificationChannel, Guid> notificationChannelRepository)
    : IQueryHandler<GetNotificationChannelQuery, NotificationChannelResponse>
{
    public async Task<Result<NotificationChannelResponse>> Handle(GetNotificationChannelQuery request, CancellationToken cancellationToken)
    {
        var entity = await notificationChannelRepository.GetByIdAsync(request.Id, cancellationToken);
        return entity is null
            ? Result.Fail<NotificationChannelResponse>(NotificationChannelError.NotFound(request.Id))
            : Result.Success(entity.ToResponse());
    }
}