namespace DataCat.Server.Application.Queries.NotificationChannel.Search;

public sealed record SearchNotificationChannelsQuery(
    int Page, 
    int PageSize, 
    string? Filter)
    : IRequest<Result<List<NotificationChannelEntity>>>, ISearchQuery;
