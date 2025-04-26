namespace DataCat.Server.Application.Commands.Alerts.Remove;

public sealed class RemoveAlertCommandValidator : AbstractValidator<RemoveAlertCommand>
{
    public RemoveAlertCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.AlertId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Alert Id must be a Guid");
                }
            });
    }
}