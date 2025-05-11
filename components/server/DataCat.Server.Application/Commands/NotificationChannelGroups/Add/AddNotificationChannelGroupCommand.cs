namespace DataCat.Server.Application.Commands.NotificationChannelGroups.Add;

public sealed record AddNotificationChannelGroupCommand(string Name) : ICommand<Guid>, IAuthorizedCommand;
