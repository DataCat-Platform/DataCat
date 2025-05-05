namespace DataCat.Server.Application.Commands.Alerts.Add;

public sealed class AddAlertCommandValidator : AbstractValidator<AddAlertCommand>
{
    public AddAlertCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.RawQuery).NotEmpty();
        RuleFor(x => x.NotificationChannelGroupName).NotEmpty();
        RuleFor(x => x.DataSourceId).NotEmpty().MustBeGuid();
        RuleFor(x => x.Tags).ForEach(y => y.NotEmpty());
    }
}