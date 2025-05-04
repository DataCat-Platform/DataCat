namespace DataCat.Server.Application.Commands.DataSources.Add;

public sealed class AddDataSourceCommandHandler(
    IDataSourceTypeRepository dataSourceTypeRepository,
    DataSourceContainer container,
    IRepository<DataSource, Guid> dataSourceRepository)
    : ICommandHandler<AddDataSourceCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddDataSourceCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var type = await dataSourceTypeRepository.GetByNameAsync(request.DataSourceType, cancellationToken);
        if (type is null)
        {
            return Result.Fail<Guid>($"DataSource type {request.DataSourceType} not found");
        }

        var purpose = DataSourcePurpose.FromName(request.Purpose.ToString(), ignoreCase: true);
        if (purpose is null)
        {
            return Result.Fail<Guid>($"DataSource purpose {request.Purpose} not found");   
        }

        var result = DataSource.Create(
            id,
            request.Name,
            type,
            request.ConnectionString,
            purpose);

        if (result.IsFailure)
            return Result.Fail<Guid>(result.Errors!);
            
        await dataSourceRepository.AddAsync(result.Value, cancellationToken);

        container.Add(purpose.DetermineKind(), result.Value);
        
        return Result.Success(id);
    }
}