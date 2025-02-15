namespace DataCat.Server.Application.Commands.Dashboard.Remove;

public sealed class RemoveDataSourceValidator : AbstractValidator<RemoveDashboardCommand>
{
    public RemoveDataSourceValidator()
    {
        RuleFor(x => x.DashboardId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Dashboard Id must be a Guid");
                }
            });
    }
}