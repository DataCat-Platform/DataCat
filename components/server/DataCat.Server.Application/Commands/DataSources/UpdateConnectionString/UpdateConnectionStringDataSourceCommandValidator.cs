namespace DataCat.Server.Application.Commands.DataSources.UpdateConnectionString;

public sealed class UpdateConnectionStringDataSourceCommandValidator : AbstractValidator<UpdateConnectionStringDataSourceCommand>
{
    public UpdateConnectionStringDataSourceCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.ConnectionString).NotEmpty();
        RuleFor(x => x.DataSourceId).NotEmpty()
            .Custom((input, context) =>
            {
                if (!Guid.TryParse(input, out _))
                {
                    context.AddFailure("DataSource Id must be a Guid");
                }
            });
    }
}