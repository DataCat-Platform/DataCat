namespace DataCat.Server.Application.Commands.Users.Remove;

public sealed class RemoveUserValidator : AbstractValidator<RemoveUserCommand>
{
    public RemoveUserValidator()
    {
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