namespace DataCat.Server.Application.Commands.Users.Add;

[Obsolete("Outdated feature")]
public sealed class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.UserName).NotEmpty();
    }
}