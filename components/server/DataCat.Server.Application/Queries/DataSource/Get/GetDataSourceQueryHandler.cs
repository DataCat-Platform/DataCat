namespace DataCat.Server.Application.Queries.DataSource.Get;

public class GetDataSourceQueryHandler(
    IRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<GetDataSourceQuery, Result<GetDataSourceResponse>>
{
    public async Task<Result<GetDataSourceResponse>> Handle(GetDataSourceQuery request, CancellationToken token)
    {
        var entity = await dataSourceRepository.GetByIdAsync(request.DataSourceId, token);
        return entity is null 
            ? Result.Fail<GetDataSourceResponse>(DataSourceError.NotFound(request.DataSourceId.ToString())) 
            : Result<GetDataSourceResponse>.Success(entity.ToResponse());
    }
}