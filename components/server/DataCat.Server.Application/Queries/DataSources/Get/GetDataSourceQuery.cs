namespace DataCat.Server.Application.Queries.DataSources.Get;

public sealed record GetDataSourceQuery(Guid DataSourceId) 
    : IRequest<Result<GetDataSourceResponse>>, IAuthorizedQuery;
