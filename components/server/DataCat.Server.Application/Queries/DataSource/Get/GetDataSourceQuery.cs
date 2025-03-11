namespace DataCat.Server.Application.Queries.DataSource.Get;

public sealed record GetDataSourceQuery(Guid DataSourceId) : IRequest<Result<DataSourceEntity>>, IAuthorizedQuery;
