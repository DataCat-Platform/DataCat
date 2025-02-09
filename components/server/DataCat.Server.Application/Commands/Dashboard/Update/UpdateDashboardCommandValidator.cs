namespace DataCat.Server.Application.Commands.Dashboard.Update;

public sealed class UpdateDashboardCommandValidator : AbstractValidator<UpdateDashboardCommand>
{
    public UpdateDashboardCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.ConnectionString).NotEmpty();
        RuleFor(x => x.DataSourceId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Plugin Id must be a Guid");
                }
            });
    }
}