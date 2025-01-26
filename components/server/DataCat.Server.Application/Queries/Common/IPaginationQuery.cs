namespace DataCat.Server.Application.Queries.Common;

public interface IPaginationQuery
{
    int Page { get; }
    int PageSize { get; }
}

public sealed class PaginationQueryValidator : AbstractValidator<IPaginationQuery>
{
    public PaginationQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}