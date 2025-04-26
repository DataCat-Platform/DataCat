namespace DataCat.Server.Application.Commands.NotificationDestinations.Remove;

public sealed class RemoveNotificationDestinationCommandValidator : AbstractValidator<RemoveNotificationDestinationCommand>
{
    public RemoveNotificationDestinationCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}