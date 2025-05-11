namespace DataCat.Server.Application.Commands.NotificationChannelGroups.Add;

public sealed class AddNotificationChannelGroupCommandValidator : AbstractValidator<AddNotificationChannelGroupCommand>
{
    public AddNotificationChannelGroupCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
    }
}