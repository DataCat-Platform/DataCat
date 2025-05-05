namespace DataCat.Server.Application.Commands.Plugins.Remove;

public sealed class RemovePluginCommandValidator : AbstractValidator<RemovePluginCommand>
{
    public RemovePluginCommandValidator()
    {
        RuleFor(x => x.PluginId).NotEmpty().MustBeGuid();
    }
}