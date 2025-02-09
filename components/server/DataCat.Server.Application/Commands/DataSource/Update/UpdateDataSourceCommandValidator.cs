namespace DataCat.Server.Application.Commands.DataSource.Update;

public sealed class UpdateDataSourceCommandValidator : AbstractValidator<UpdateDataSourceCommand>
{
    public UpdateDataSourceCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.ConnectionString).NotEmpty();
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