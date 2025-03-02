namespace DataCat.Server.Application.Commands.DataSource.UpdateConnectionString;

public sealed class UpdateConnStringDataSourceCommandHandler(
    IDefaultRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<UpdateConnStringDataSourceCommand, Result>
{
    public async Task<Result> Handle(UpdateConnStringDataSourceCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.DataSourceId);
        
        var dataSource = await dataSourceRepository.GetByIdAsync(id, cancellationToken);
        if (dataSource is null)
            return Result.Fail(DataSourceError.NotFound(id.ToString()));
        
        dataSource.ChangeConnectionString(request.ConnectionString);
        await dataSourceRepository.UpdateAsync(dataSource, cancellationToken);
        
        return Result.Success();
    }
}