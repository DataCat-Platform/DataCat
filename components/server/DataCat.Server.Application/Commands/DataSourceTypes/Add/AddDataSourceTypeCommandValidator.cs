namespace DataCat.Server.Application.Commands.DataSourceTypes.Add;

public sealed class AddDataSourceTypeCommandValidator : AbstractValidator<AddDataSourceTypeCommand>
{
    public AddDataSourceTypeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}