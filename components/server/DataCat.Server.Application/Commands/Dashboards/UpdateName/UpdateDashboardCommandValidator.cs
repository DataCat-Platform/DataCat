namespace DataCat.Server.Application.Commands.Dashboards.UpdateName;

public sealed class UpdateDashboardCommandValidator : AbstractValidator<UpdateDashboardCommand>
{
    public UpdateDashboardCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.DashboardId).NotEmpty().MustBeGuid();
    }
}