namespace DataCat.Server.Application.Commands.NotificationChannels.UpdateNotificationQuery;

public sealed class UpdateNotificationCommandValidator : AbstractValidator<UpdateNotificationCommand>
{
    public UpdateNotificationCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Settings).NotEmpty().NotNull();
        RuleFor(x => x.DestinationTypeName).NotEmpty();
    }
}