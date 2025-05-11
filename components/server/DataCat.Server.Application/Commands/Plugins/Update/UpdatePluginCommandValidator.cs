namespace DataCat.Server.Application.Commands.Plugins.Update;

public class UpdatePluginCommandValidator : AbstractValidator<UpdatePluginCommand>
{
    public UpdatePluginCommandValidator()
    {
        RuleFor(x => x.PluginId).NotEmpty().MustBeGuid();
    }
}