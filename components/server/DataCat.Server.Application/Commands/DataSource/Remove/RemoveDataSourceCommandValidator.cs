namespace DataCat.Server.Application.Commands.DataSource.Remove;

public sealed class RemoveDataSourceValidator : AbstractValidator<RemoveDataSourceCommand>
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