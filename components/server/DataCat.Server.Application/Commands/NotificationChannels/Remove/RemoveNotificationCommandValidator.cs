namespace DataCat.Server.Application.Commands.NotificationChannels.Remove;

public sealed class RemoveNotificationCommandValidator : AbstractValidator<RemoveNotificationCommand>
{
    public RemoveNotificationCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.NotificationId).NotEmpty().MustBeGuid();
    }
}