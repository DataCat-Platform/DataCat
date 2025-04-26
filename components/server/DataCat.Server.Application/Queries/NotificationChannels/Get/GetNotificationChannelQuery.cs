namespace DataCat.Server.Application.Queries.NotificationChannels.Get;

public sealed record GetNotificationChannelQuery(Guid NotificationChannelId)
    : IRequest<Result<GetNotificationChannelResponse>>, IAuthorizedQuery;
