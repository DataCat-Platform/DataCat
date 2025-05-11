namespace DataCat.Server.Application.Commands.DataSources.Remove;

public sealed class RemoveDataSourceCommandHandler(
    IRepository<DataSource, Guid> repository,
    IDataSourceRepository dataSourceRepository,
    DataSourceContainer container)
    : ICommandHandler<RemoveDataSourceCommand>
{
    public async Task<Result> Handle(RemoveDataSourceCommand request, CancellationToken token)
    {
        var dataSource = await repository.GetByIdAsync(request.Id, token);
        if (dataSource is null)
            return Result.Fail(DataSourceError.NotFoundById(request.Id.ToString()));
        
        await dataSourceRepository.DeleteAsync(dataSource.Id, token);
        
        container.Remove(dataSource.Purpose.DetermineKind(), dataSource.Name);
        
        return Result.Success();
    }
}