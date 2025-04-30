namespace DataCat.Server.Application.Commands.Alerts.Update;

public sealed class UpdateAlertQueryCommandValidator : AbstractValidator<UpdateAlertQueryCommand>
{
    public UpdateAlertQueryCommandValidator()
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
        RuleFor(x => x.RawQuery).NotEmpty();
        RuleFor(x => x.DataSourceId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("DataSource Id must be a Guid");
                }
            });
        RuleFor(x => x.NotificationChannelId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("NotificationChannel Id must be a Guid");
                }
            });
    }
}