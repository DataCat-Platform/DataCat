namespace DataCat.Server.Application.Commands.Panel.Remove;

public sealed class RemovePanelValidator : AbstractValidator<RemovePanelCommand>
{
    public RemovePanelValidator()
    {
        RuleFor(x => x.PanelId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Panel Id must be a Guid");
                }
            });
    }
}