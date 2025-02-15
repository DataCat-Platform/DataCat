namespace DataCat.Server.Application.Commands.Plugin.ToggleStatus;

public sealed class ToggleStatusCommandValidator : AbstractValidator<ToggleStatusCommand>
{
    public ToggleStatusCommandValidator()
    {
        RuleFor(x => x.PluginId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Plugin Id must be a Guid");
                }
            });
        RuleFor(x => x.ToggleStatus).NotNull()
            .Must(status => status is ToggleStatus.Active or ToggleStatus.InActive)
            .WithMessage("Status must be either 'Active' or 'InActive'");
    }
}