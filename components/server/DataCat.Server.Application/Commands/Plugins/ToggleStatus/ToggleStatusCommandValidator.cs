namespace DataCat.Server.Application.Commands.Plugins.ToggleStatus;

public sealed class ToggleStatusCommandValidator : AbstractValidator<ToggleStatusCommand>
{
    public ToggleStatusCommandValidator()
    {
        RuleFor(x => x.PluginId).NotEmpty().MustBeGuid();
        RuleFor(x => x.ToggleStatus).NotNull()
            .Must(status => status is ToggleStatus.Active or ToggleStatus.InActive)
            .WithMessage("Status must be either 'Active' or 'InActive'");
    }
}