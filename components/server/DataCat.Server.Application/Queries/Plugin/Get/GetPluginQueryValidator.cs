namespace DataCat.Server.Application.Queries.Plugin.Get;

public sealed class GetPluginQueryValidator : AbstractValidator<GetPluginQuery>
{
    public GetPluginQueryValidator()
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