namespace DataCat.Server.Application.Queries.DataSources.Get;

public sealed record GetDataSourceQuery(Guid DataSourceId) 
    : IQuery<GetDataSourceResponse>, IAuthorizedQuery;
