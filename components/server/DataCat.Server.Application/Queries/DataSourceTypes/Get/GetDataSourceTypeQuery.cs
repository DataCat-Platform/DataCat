namespace DataCat.Server.Application.Queries.DataSourceTypes.Get;

public sealed record GetDataSourceTypeQuery(string Name)
    : IQuery<GetDataSourceTypeResponse>, IAuthorizedQuery;
