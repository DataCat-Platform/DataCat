namespace DataCat.Server.Application.Queries.NotificationChannels.Get;

public sealed record GetNotificationChannelQuery(int Id) : IQuery<NotificationChannelResponse>, IAuthorizedQuery;
