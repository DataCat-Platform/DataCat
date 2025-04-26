namespace DataCat.Server.Application.Commands.DataSources.Remove;

public sealed class RemoveDataSourceValidator : AbstractValidator<RemoveDataSourceCommand>
{
    public RemoveDataSourceValidator()
    {
        RuleFor(x => x.DataSourceName).NotEmpty();
    }
}