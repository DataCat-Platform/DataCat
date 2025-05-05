namespace DataCat.Server.Application.Commands.NotificationDestinations.Add;

public sealed class AddNotificationDestinationCommandValidator : AbstractValidator<AddNotificationDestinationCommand>
{
    public AddNotificationDestinationCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}