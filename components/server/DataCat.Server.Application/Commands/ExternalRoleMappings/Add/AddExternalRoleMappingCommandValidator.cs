namespace DataCat.Server.Application.Commands.ExternalRoleMappings.Add;

public sealed class AddExternalRoleMappingCommandValidator : AbstractValidator<AddExternalRoleMappingCommand>
{
    public AddExternalRoleMappingCommandValidator()
    {
        RuleFor(x => x.RoleId).Custom((input, context) =>
        {
            if (!UserRole.TryFromValue(input, out _))
            {
                context.AddFailure("Cannot parse user role");
            }
        });

        RuleFor(x => x.ExternalRole).NotEmpty();
        RuleFor(x => x.NamespaceId).MustBeGuid();
    }
}