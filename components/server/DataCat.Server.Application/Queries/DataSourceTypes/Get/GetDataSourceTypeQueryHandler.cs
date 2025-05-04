namespace DataCat.Server.Application.Queries.DataSourceTypes.Get;

public sealed class GetDataSourceTypeQueryHandler(
    IDataSourceTypeRepository dataSourceTypeRepository) 
    : IQueryHandler<GetDataSourceTypeQuery, GetDataSourceTypeResponse>
{
    public async Task<Result<GetDataSourceTypeResponse>> Handle(GetDataSourceTypeQuery request, CancellationToken cancellationToken)
    {
        var dataSourceType = await dataSourceTypeRepository.GetByNameAsync(request.Name, cancellationToken);
        return dataSourceType is null 
            ? Result.Fail<GetDataSourceTypeResponse>(DataSourceTypeError.NotFound(request.Name)) 
            : Result.Success(new GetDataSourceTypeResponse(dataSourceType.Id, dataSourceType.Name));
    }
}