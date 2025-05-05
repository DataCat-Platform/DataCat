namespace DataCat.Server.Application.Queries.NotificationChannels.Get;

public sealed record GetNotificationChannelQuery(Guid Id) : IQuery<NotificationChannelResponse>, IAuthorizedQuery;
