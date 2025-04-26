namespace DataCat.Server.Application.Commands.Users.Add;

public sealed class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.UserName).NotEmpty();
    }
}