namespace DataCat.Server.Application.Commands.Panels.Update;

public sealed class UpdatePanelCommandValidator : AbstractValidator<UpdatePanelCommand>
{
    public UpdatePanelCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.PanelId).NotEmpty().MustBeGuid();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Type).GreaterThan(0);
        RuleFor(x => x.RawQuery).NotEmpty();
        RuleFor(x => x.DataSourceId).NotEmpty().MustBeGuid();
    }
}