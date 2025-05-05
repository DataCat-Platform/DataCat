namespace DataCat.Server.Application.Commands.NotificationChannels.UpdateNotificationQuery;

public sealed class UpdateNotificationCommandValidator : AbstractValidator<UpdateNotificationCommand>
{
    public UpdateNotificationCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.NotificationChannelId).NotEmpty().MustBeGuid();
        RuleFor(x => x.Settings).NotEmpty().NotNull();
        RuleFor(x => x.DestinationTypeName).NotEmpty();
    }
}