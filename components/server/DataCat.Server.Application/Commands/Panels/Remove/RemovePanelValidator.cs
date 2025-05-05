namespace DataCat.Server.Application.Commands.Panels.Remove;

public sealed class RemovePanelValidator : AbstractValidator<RemovePanelCommand>
{
    public RemovePanelValidator()
    {
        RuleFor(x => x.PanelId).NotEmpty().MustBeGuid();
    }
}