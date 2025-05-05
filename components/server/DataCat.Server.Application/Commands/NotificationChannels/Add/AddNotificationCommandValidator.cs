namespace DataCat.Server.Application.Commands.NotificationChannels.Add;

public sealed class AddNotificationCommandValidator : AbstractValidator<AddNotificationCommand>
{
    public AddNotificationCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Settings).NotEmpty().NotNull();
        RuleFor(x => x.DestinationTypeName).NotEmpty();
    }
}