namespace DataCat.Server.Application.Commands.Variables.Update;

public sealed class UpdateVariableCommandValidator : AbstractValidator<UpdateVariableCommand>
{
    public UpdateVariableCommandValidator()
    {
        RuleFor(x => x.Value).NotEmpty();
        RuleFor(x => x.Placeholder).NotEmpty();
    }
}