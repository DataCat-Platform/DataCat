namespace DataCat.Server.Application.Commands.Variables.Add;

public sealed class AddVariableCommandValidator : AbstractValidator<AddVariableCommand>
{
    public AddVariableCommandValidator()
    {
        RuleFor(x => x.Value).NotEmpty();
        RuleFor(x => x.Placeholder).NotEmpty();
    }
}