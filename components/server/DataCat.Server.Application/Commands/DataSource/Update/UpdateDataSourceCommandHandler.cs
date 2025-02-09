namespace DataCat.Server.Application.Commands.DataSource.Update;

public sealed class UpdateDataSourceCommandHandler(
    IDefaultRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<UpdateDataSourceCommand, Result>
{
    public async Task<Result> Handle(UpdateDataSourceCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.DataSourceId);
        
        var dataSource = await dataSourceRepository.GetByIdAsync(id, cancellationToken);
        if (dataSource is null)
            return Result.Fail(DataSourceError.NotFound(id.ToString()));
        
        dataSource.ChangeConnnectionString(request.ConnectionString);
        await dataSourceRepository.UpdateAsync(dataSource, cancellationToken);
        
        return Result.Success();
    }
}