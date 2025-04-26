namespace DataCat.Server.Application.Commands.Dashboards.UpdateName;

public sealed class UpdateDashboardCommandValidator : AbstractValidator<UpdateDashboardCommand>
{
    public UpdateDashboardCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.DashboardId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Dashboard Id must be a Guid");
                }
            });
    }
}