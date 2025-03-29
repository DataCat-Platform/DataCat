namespace DataCat.Server.Application.Commands.DataSource.Add;

public sealed class AddDataSourceCommandHandler(
    IRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<AddDataSourceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddDataSourceCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var result = CreateDataSource(id, request);

        if (result.IsFailure)
            return Result.Fail<Guid>(result.Errors!);
            
        await dataSourceRepository.AddAsync(result.Value, cancellationToken);
        return Result.Success(id);
    }
    
    private static Result<DataSourceEntity> CreateDataSource(Guid id, AddDataSourceCommand request)
    {
        var type = DataSourceType.FromValue(request.Type);
        if (type is null)
        {
            return Result.Fail<DataSourceEntity>($"DataSource type {request.Type} not found");
        }

        return DataSourceEntity.Create(
            id,
            request.Name,
            type,
            request.ConnectionString);
    }
}