namespace DataCat.Server.Application.Commands.Dashboards.Add;

public sealed class AddDashboardCommandValidator : AbstractValidator<AddDashboardCommand>
{
    public AddDashboardCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty().MustBeGuid();
        RuleForEach(x => x.Tags).ChildRules(x =>
        {
            x.RuleFor(t => t).NotEmpty();
        });
    }
}