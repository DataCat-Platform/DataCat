namespace DataCat.Server.Application.Logs.Queries.Search;

public sealed class LogSearchQueryValidator : AbstractValidator<LogSearchQuery> 
{
    public LogSearchQueryValidator()
    {
        Include(new PaginationQueryValidator());
    }
}