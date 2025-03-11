namespace DataCat.Server.Application.Queries.NotificationChannel.Get;

public sealed record GetNotificationChannelQuery(Guid NotificationChannelId) : IRequest<Result<NotificationChannelEntity>>, IAuthorizedQuery;
