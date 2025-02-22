namespace DataCat.Server.Application.Queries.NotificationChannel.Search;

public class SearchNotificationChannelsQueryHandler(
    IDefaultRepository<NotificationChannelEntity, Guid> notificationChannelsRepository)
    : IRequestHandler<SearchNotificationChannelsQuery, Result<List<NotificationChannelEntity>>>
{
    public async Task<Result<List<NotificationChannelEntity>>> Handle(SearchNotificationChannelsQuery request, CancellationToken token)
    {
        var result = await notificationChannelsRepository
            .SearchAsync(request.Filter, request.Page, request.PageSize, token)
            .ToListAsync(cancellationToken: token);
        return Result.Success(result);
    }
}