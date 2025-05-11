namespace DataCat.Server.Application.Commands.DataSources.Add;

public sealed class AddDataSourceCommandValidator : AbstractValidator<AddDataSourceCommand>
{
    public AddDataSourceCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ConnectionString).NotEmpty();
        RuleFor(x => x.DataSourceType).NotEmpty();
    }
}