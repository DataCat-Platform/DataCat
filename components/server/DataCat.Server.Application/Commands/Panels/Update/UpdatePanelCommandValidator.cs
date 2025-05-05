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
        RuleFor(x => x.PanelX).GreaterThan(0);
        RuleFor(x => x.PanelY).GreaterThan(0);
        RuleFor(x => x.Width).GreaterThan(0);
        RuleFor(x => x.Height).GreaterThan(0);
    }
}