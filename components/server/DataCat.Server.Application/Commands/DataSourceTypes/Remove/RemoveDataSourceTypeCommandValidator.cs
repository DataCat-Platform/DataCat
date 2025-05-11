namespace DataCat.Server.Application.Commands.DataSourceTypes.Remove;

public sealed class RemoveDataSourceTypeCommandValidator : AbstractValidator<RemoveDataSourceTypeCommand>
{
    public RemoveDataSourceTypeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}