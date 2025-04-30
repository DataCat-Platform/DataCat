namespace DataCat.Server.Application.Commands.Plugins.Add;

public sealed class AddPluginCommandValidator : AbstractValidator<AddPluginCommand>
{
    public AddPluginCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Version).NotEmpty();
        RuleFor(x => x.File).NotNull();
    }
}