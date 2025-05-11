namespace DataCat.Server.Application.Queries.DataSourceTypes.GetAll;

public sealed class GetAllDataSourceTypesQueryHandler(
    IDataSourceTypeRepository dataSourceTypeRepository) 
    : IQueryHandler<GetAllDataSourceTypesQuery, List<GetDataSourceTypeResponse>>
{
    public async Task<Result<List<GetDataSourceTypeResponse>>> Handle(GetAllDataSourceTypesQuery request, CancellationToken cancellationToken)
    {
        var result = await dataSourceTypeRepository.GetAllAsync(cancellationToken);
        return Result.Success(result.Select(x => new GetDataSourceTypeResponse(x.Id, x.Name)).ToList());
    }
}