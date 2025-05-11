namespace DataCat.Server.Application.Queries.Common;

public sealed record SearchFilters
{
    public IReadOnlyCollection<SearchFilter> Filters { get; init; } = [];
    public Sort? Sort { get; init; } = null;
}

public sealed record SearchFilter(
    string Key,
    object Value,
    MatchMode MatchMode,
    SearchFieldType FieldType);
    
public enum MatchMode
{
    Equals = 1,
    StartsWith = 2,
    Contains = 3,
}

public enum SearchFieldType
{
    String = 0,
    Number = 1,
    Date = 2,
    Boolean = 3,
    Array = 4,
}

public sealed record Sort(string FieldName, SortOrder SortOrder = SortOrder.Asc);

public enum SortOrder
{
    Asc = 1,
    Desc = -1
}