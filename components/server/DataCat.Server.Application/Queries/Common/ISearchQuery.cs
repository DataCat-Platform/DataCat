namespace DataCat.Server.Application.Queries.Common;

public interface ISearchQuery : IPaginationQuery
{
    string? Filter { get; }
}

public sealed class SearchQueryValidator : AbstractValidator<ISearchQuery>;