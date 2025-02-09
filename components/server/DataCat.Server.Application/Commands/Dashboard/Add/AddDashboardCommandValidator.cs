namespace DataCat.Server.Application.Commands.Dashboard.Add;

public sealed class AddDashboardCommandValidator : AbstractValidator<AddDashboardCommand>
{
    public AddDashboardCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Plugin Id must be a Guid");
                }
            });
    }
}