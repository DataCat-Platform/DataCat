namespace DataCat.Server.Application.Commands.DataSources.Add;

public sealed class AddDataSourceCommandHandler(
    IDataSourceTypeRepository dataSourceTypeRepository,
    IRepository<DataSource, Guid> dataSourceRepository)
    : IRequestHandler<AddDataSourceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddDataSourceCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var type = await dataSourceTypeRepository.GetByNameAsync(request.DataSourceType, cancellationToken);
        if (type is null)
        {
            return Result.Fail<Guid>($"DataSource type {request.DataSourceType} not found");
        }

        var result = DataSource.Create(
            id,
            request.Name,
            type,
            request.ConnectionString);

        if (result.IsFailure)
            return Result.Fail<Guid>(result.Errors!);
            
        await dataSourceRepository.AddAsync(result.Value, cancellationToken);
        return Result.Success(id);
    }
}