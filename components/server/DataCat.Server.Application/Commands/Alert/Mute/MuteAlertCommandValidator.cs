namespace DataCat.Server.Application.Commands.Alert.Mute;

public sealed class MuteAlertCommandValidator : AbstractValidator<MuteAlertCommand>
{
    public MuteAlertCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Id).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Alert Id must be a Guid");
                }
            });
        RuleFor(x => x.NextExecutionTime).GreaterThan(TimeSpan.Zero);
    }
}