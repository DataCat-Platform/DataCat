namespace DataCat.Server.Application.Commands.DataSourceTypes.Add;

public sealed class AddDataSourceTypeCommandHandler(
    IDataSourceTypeRepository dataSourceTypeRepository) 
    : IRequestHandler<AddDataSourceTypeCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AddDataSourceTypeCommand request, CancellationToken cancellationToken)
    {
        var dataSourceType = await dataSourceTypeRepository.GetByNameAsync(request.Name, cancellationToken);
        if (dataSourceType is not null)
            return Result.Success(dataSourceType.Id);
        
        var dataSourceTypeResult = DataSourceType.Create(request.Name);
        if (dataSourceTypeResult.IsFailure)
            return Result.Fail<int>(dataSourceTypeResult.Errors!);
        
        var id = await dataSourceTypeRepository.AddAsync(dataSourceTypeResult.Value, cancellationToken);
        return Result.Success(id);
    }
}