namespace DataCat.Server.Application.Commands.NotificationChannel.UpdateNotificationQuery;

public sealed class UpdateNotificationCommandValidator : AbstractValidator<UpdateNotificationCommand>
{
    public UpdateNotificationCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.NotificationChannelId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("NotificationChannel Id must be a Guid");
                }
            });
        RuleFor(x => x.Settings).NotEmpty().NotNull();
        RuleFor(x => x.DestinationType).GreaterThan(0);
    }
}