namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record RawQueryResponse
{
    public required string Query { get; init; }
    public required DataSourceResponse DataSource { get; init; }
}

public static class RawQueryResponseExtensions
{
    public static RawQueryResponse ToResponse(this Query query)
    {
        return new RawQueryResponse
        {
            Query = query.RawQuery,
            DataSource = query.DataSource.ToResponse()
        };
    }
}