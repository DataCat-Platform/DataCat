namespace DataCat.Server.Application.Commands.Dashboard.AddUser;

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