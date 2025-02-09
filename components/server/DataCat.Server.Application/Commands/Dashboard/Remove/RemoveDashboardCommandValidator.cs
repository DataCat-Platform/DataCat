namespace DataCat.Server.Application.Commands.Dashboard.Remove;

public sealed class RemoveDataSourceValidator : AbstractValidator<RemoveDashboardCommand>
{
    public RemoveDataSourceValidator()
    {
        RuleFor(x => x.DataSourceId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("Plugin Id must be a Guid");
                }
            });
    }
}