namespace DataCat.Server.Application.Queries.Common;

public sealed record Page<T>(
    IReadOnlyCollection<T> Items, 
    int TotalCount, 
    int PageNumber, 
    int PageSize)
{
    public int TotalPages { get; } = (int)Math.Ceiling(TotalCount / (double)PageSize);

    public int PageSize { get; set; } = PageSize;

    public bool HasPreviousPage => PageNumber > 1; 
 
    public bool HasNextPage => PageNumber < TotalPages; 
}