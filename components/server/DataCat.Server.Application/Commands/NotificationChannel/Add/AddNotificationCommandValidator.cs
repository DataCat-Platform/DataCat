namespace DataCat.Server.Application.Commands.NotificationChannel.Add;

public sealed class AddNotificationCommandValidator : AbstractValidator<AddNotificationCommand>
{
    public AddNotificationCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Settings).NotEmpty();
        RuleFor(x => x.DestinationType).GreaterThan(0);
    }
}