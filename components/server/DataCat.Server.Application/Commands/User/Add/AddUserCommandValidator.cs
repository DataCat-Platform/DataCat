namespace DataCat.Server.Application.Commands.User.Add;

public sealed class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.RoleId).GreaterThan(0);
    }
}