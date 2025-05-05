namespace DataCat.Server.Application.Commands.Alerts.Update;

public sealed class UpdateAlertQueryCommandValidator : AbstractValidator<UpdateAlertQueryCommand>
{
    public UpdateAlertQueryCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.AlertId).NotEmpty().MustBeGuid();
        RuleFor(x => x.RawQuery).NotEmpty();
        RuleFor(x => x.DataSourceId).NotEmpty().MustBeGuid();
        RuleFor(x => x.NotificationChannelId).NotEmpty().MustBeGuid();
    }
}