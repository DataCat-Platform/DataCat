namespace DataCat.Server.Application.Commands.Users.GetAccessTokenByCode;

public sealed class GetAccessTokenByCodeCommandValidator : AbstractValidator<GetAccessTokenByCodeCommand>
{
    public GetAccessTokenByCodeCommandValidator()
    {
        RuleFor(v => v.Code).NotEmpty();
    }
}