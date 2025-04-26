namespace DataCat.Server.Application.Commands.DataSourceTypes.Remove;

public sealed class RemoveDataSourceTypeCommandHandler(
    IDataSourceTypeRepository dataSourceTypeRepository) 
    : IRequestHandler<RemoveDataSourceTypeCommand, Result>
{
    public async Task<Result> Handle(RemoveDataSourceTypeCommand request, CancellationToken cancellationToken)
    {
        var dataSourceType = await dataSourceTypeRepository.GetByNameAsync(request.Name, cancellationToken);
        if (dataSourceType is null)
            return Result.Fail(DataSourceTypeError.NotFound(request.Name));
        
        await dataSourceTypeRepository.DeleteAsync(dataSourceType.Id, cancellationToken);
        return Result.Success();
    }
}