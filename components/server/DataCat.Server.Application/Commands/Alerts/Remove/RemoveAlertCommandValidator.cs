namespace DataCat.Server.Application.Commands.Alerts.Remove;

public sealed class RemoveAlertCommandValidator : AbstractValidator<RemoveAlertCommand>
{
    public RemoveAlertCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.AlertId).NotEmpty().MustBeGuid();
    }
}