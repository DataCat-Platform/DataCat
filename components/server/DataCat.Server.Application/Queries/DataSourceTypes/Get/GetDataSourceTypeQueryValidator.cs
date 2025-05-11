namespace DataCat.Server.Application.Queries.DataSourceTypes.Get;

public sealed class GetDataSourceTypeQueryValidator : AbstractValidator<GetDataSourceTypeQuery>
{
    public GetDataSourceTypeQueryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}