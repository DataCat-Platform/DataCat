namespace DataCat.Server.Application.Queries.DataSourceTypes.GetAll;

public sealed record GetAllDataSourceTypesQuery : IQuery<List<GetDataSourceTypeResponse>>, IAuthorizedQuery;
