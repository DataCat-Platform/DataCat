namespace DataCat.Server.Application.Commands.Alerts.Mute;

public sealed class MuteAlertCommandValidator : AbstractValidator<MuteAlertCommand>
{
    public MuteAlertCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Id).NotEmpty().MustBeGuid();
        RuleFor(x => x.NextExecutionTime).GreaterThan(TimeSpan.Zero);
    }
}