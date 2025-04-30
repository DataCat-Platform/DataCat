namespace DataCat.Server.Application.Commands.DataSources.Remove;

public sealed class RemoveDataSourceCommandHandler(
    IDataSourceRepository dataSourceRepository,
    DataSourceContainer container)
    : IRequestHandler<RemoveDataSourceCommand, Result>
{
    public async Task<Result> Handle(RemoveDataSourceCommand request, CancellationToken token)
    {
        var dataSource = await dataSourceRepository.GetByNameAsync(request.DataSourceName, token);
        if (dataSource is null)
            return Result.Fail(DataSourceError.NotFoundByName(request.DataSourceName));
        
        await dataSourceRepository.DeleteAsync(dataSource.Id, token);
        
        container.Remove(dataSource.Purpose.DetermineKind(), dataSource.Name);
        
        return Result.Success();
    }
}