namespace DataCat.Server.Application.Commands.Panels.Add;

public sealed class AddPanelCommandValidator : AbstractValidator<AddPanelCommand>
{
    public AddPanelCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Type).GreaterThan(0);
        RuleFor(x => x.RawQuery).NotEmpty();
        RuleFor(x => x.DataSourceId).NotEmpty().MustBeGuid();
        RuleFor(x => x.DashboardId).NotEmpty().MustBeGuid();
    }
}