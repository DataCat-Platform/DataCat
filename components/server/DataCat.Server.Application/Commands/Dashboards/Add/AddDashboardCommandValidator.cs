namespace DataCat.Server.Application.Commands.Dashboards.Add;

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
                    context.AddFailure("User Id must be a Guid");
                }
            });
    }
}