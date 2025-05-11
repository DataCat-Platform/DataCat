namespace DataCat.Server.Application.Queries.Common;

public sealed record Page<T>(
    IEnumerable<T> Items, 
    long TotalCount, 
    int PageNumber, 
    int PageSize)
{
    public int TotalPages { get; } = (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => PageNumber > 1; 
 
    public bool HasNextPage => PageNumber < TotalPages;

    public Page<TOutput> ToResponsePage<TOutput>(Func<T, TOutput> mapper)
        => new(Items.Select(mapper).ToArray(), TotalCount, PageNumber, PageSize);
}
