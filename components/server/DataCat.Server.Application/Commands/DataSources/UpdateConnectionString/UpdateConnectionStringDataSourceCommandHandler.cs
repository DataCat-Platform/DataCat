namespace DataCat.Server.Application.Commands.DataSources.UpdateConnectionString;

public sealed class UpdateConnectionStringDataSourceCommandHandler(
    IRepository<DataSource, Guid> dataSourceBaseRepository,
    IDataSourceRepository dataSourceRepository,
    DataSourceContainer container)
    : ICommandHandler<UpdateConnectionStringDataSourceCommand>
{
    public async Task<Result> Handle(UpdateConnectionStringDataSourceCommand request, CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceBaseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (dataSource is null)
            return Result.Fail(DataSourceError.NotFoundById(request.Id.ToString()));
        
        dataSource.ChangeConnectionString(request.ConnectionString);
        await dataSourceRepository.UpdateAsync(dataSource, cancellationToken);

        var dataSourceFromContainer = container.Find(dataSource.Purpose.DetermineKind(), dataSource.Name);
        if (dataSourceFromContainer is null)
            return Result.Fail(DataSourceError.NotFoundById(request.Id.ToString()));
        
        dataSourceFromContainer.ChangeConnectionString(request.ConnectionString);
        
        return Result.Success();
    }
}