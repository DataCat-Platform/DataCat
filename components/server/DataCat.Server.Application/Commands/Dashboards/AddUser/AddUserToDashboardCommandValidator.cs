namespace DataCat.Server.Application.Commands.Dashboards.AddUser;

public sealed class AddUserToDashboardCommandValidator : AbstractValidator<AddUserToDashboardCommand>
{
    public AddUserToDashboardCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.DashboardId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Dashboard Id must be a Guid");
                }
            });
        RuleFor(x => x.UserId).NotEmpty().MustBeGuid();
    }
}