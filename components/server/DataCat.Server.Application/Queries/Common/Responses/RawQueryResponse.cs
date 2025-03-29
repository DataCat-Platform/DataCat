namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record RawQueryResponse
{
    public required string Query { get; init; }
    public required DataSourceResponse DataSource { get; init; }
}

public static class RawQueryResponseExtensions
{
    public static RawQueryResponse ToResponse(this QueryEntity queryEntity)
    {
        return new RawQueryResponse
        {
            Query = queryEntity.RawQuery,
            DataSource = queryEntity.DataSourceEntity.ToResponse()
        };
    }
}