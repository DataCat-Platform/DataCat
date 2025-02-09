namespace DataCat.Server.Application.Commands.Dashboard.Update;

public sealed class UpdateDashboardCommandHandler(
    IDefaultRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<UpdateDashboardCommand, Result>
{
    public async Task<Result> Handle(UpdateDashboardCommand request, CancellationToken cancellationToken)
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