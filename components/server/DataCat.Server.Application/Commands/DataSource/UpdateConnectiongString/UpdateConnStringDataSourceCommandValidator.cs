namespace DataCat.Server.Application.Commands.DataSource.UpdateConnectiongString;

public sealed class UpdateConnStringDataSourceCommandValidator : AbstractValidator<UpdateConnStringDataSourceCommand>
{
    public UpdateConnStringDataSourceCommandValidator()
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