namespace DataCat.Server.Application.Commands.Alert.Add;

public sealed class AddAlertCommandValidator : AbstractValidator<AddAlertCommand>
{
    public AddAlertCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
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