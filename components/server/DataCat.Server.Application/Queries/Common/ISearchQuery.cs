namespace DataCat.Server.Application.Queries.Common;

public interface ISearchQuery : IPaginationQuery
{
    string? Filter { get; }
}

public sealed class SearchQueryValidator : AbstractValidator<ISearchQuery>
{
    public SearchQueryValidator()
    {
        RuleFor(x => x.Filter).NotNull().NotEmpty();
    }
}