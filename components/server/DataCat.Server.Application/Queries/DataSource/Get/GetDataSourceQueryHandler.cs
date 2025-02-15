namespace DataCat.Server.Application.Queries.DataSource.Get;

public class GetDataSourceQueryHandler(
    IDefaultRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<GetDataSourceQuery, Result<DataSourceEntity>>
{
    public async Task<Result<DataSourceEntity>> Handle(GetDataSourceQuery request, CancellationToken token)
    {
        var entity = await dataSourceRepository.GetByIdAsync(request.DataSourceId, token);
        return entity is null 
            ? Result.Fail<DataSourceEntity>(DataSourceError.NotFound(request.DataSourceId.ToString())) 
            : Result<DataSourceEntity>.Success(entity);
    }
}