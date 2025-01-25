namespace DataCat.Server.Application.Commands.Plugin.Remove;

public sealed class RemovePluginCommandValidator : AbstractValidator<RemovePluginCommand>
{
    public RemovePluginCommandValidator()
    {
        RuleFor(x => x.PluginId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Plugin Id must be a Guid");
                }
            });
    }
}