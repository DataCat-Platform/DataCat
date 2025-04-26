namespace DataCat.Server.Application.Commands.Panels.Add;

public sealed class AddPanelCommandValidator : AbstractValidator<AddPanelCommand>
{
    public AddPanelCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Type).GreaterThan(0);
        RuleFor(x => x.RawQuery).NotEmpty();
        
        RuleFor(x => x.DataSourceId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("DataSource Id must be a Guid");
                }
            });
        RuleFor(x => x.PanelX).GreaterThan(0);
        RuleFor(x => x.PanelY).GreaterThan(0);
        RuleFor(x => x.Width).GreaterThan(0);
        RuleFor(x => x.Height).GreaterThan(0);
        
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