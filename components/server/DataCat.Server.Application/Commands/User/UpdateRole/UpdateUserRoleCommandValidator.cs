namespace DataCat.Server.Application.Commands.User.UpdateRole;

public sealed class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserInfoCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
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