namespace DataCat.Server.Application.Commands.NotificationChannels.Remove;

public sealed class RemoveNotificationCommandValidator : AbstractValidator<RemoveNotificationCommand>
{
    public RemoveNotificationCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.NotificationId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Notification Id must be a Guid");
                }
            });
    }
}