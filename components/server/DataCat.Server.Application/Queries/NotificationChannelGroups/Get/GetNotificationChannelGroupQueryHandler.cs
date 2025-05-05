namespace DataCat.Server.Application.Queries.NotificationChannelGroups.Get;

public class GetNotificationChannelGroupQueryHandler(
    INotificationChannelGroupRepository notificationChannelGroupRepository)
    : IQueryHandler<GetNotificationChannelGroupQuery, NotificationChannelGroupResponse>
{
    public async Task<Result<NotificationChannelGroupResponse>> Handle(GetNotificationChannelGroupQuery request,
        CancellationToken token)
    {
        var entity = await notificationChannelGroupRepository.GetByName(request.Name, token);
        return entity is null
            ? Result.Fail<NotificationChannelGroupResponse>(NotificationChannelGroupError.NotFound(request.Name))
            : Result.Success(entity.ToResponse());
    }
}