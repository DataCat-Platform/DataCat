namespace DataCat.Server.Application.Commands.DataSources.UpdateConnectionString;

public sealed class UpdateConnectionStringDataSourceCommandHandler(
    IRepository<DataSource, Guid> dataSourceBaseRepository,
    IDataSourceRepository dataSourceRepository)
    : IRequestHandler<UpdateConnectionStringDataSourceCommand, Result>
{
    public async Task<Result> Handle(UpdateConnectionStringDataSourceCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.DataSourceId);
        
        var dataSource = await dataSourceBaseRepository.GetByIdAsync(id, cancellationToken);
        if (dataSource is null)
            return Result.Fail(DataSourceError.NotFound(id.ToString()));
        
        dataSource.ChangeConnectionString(request.ConnectionString);
        await dataSourceRepository.UpdateAsync(dataSource, cancellationToken);
        
        return Result.Success();
    }
}