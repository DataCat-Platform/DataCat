namespace DataCat.Server.Application.Commands.Plugin.Update;

public class UpdatePluginCommandValidator : AbstractValidator<UpdatePluginCommand>
{
    public UpdatePluginCommandValidator()
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