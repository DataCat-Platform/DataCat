namespace DataCat.Server.Application.Commands.Users.Remove;

[Obsolete("Outdated feature")]
public sealed class RemoveUserValidator : AbstractValidator<RemoveUserCommand>
{
    public RemoveUserValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().MustBeGuid();
    }
}