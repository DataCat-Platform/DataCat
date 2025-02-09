namespace DataCat.Server.Application.Commands.Dashboard.Remove;

public sealed class RemoveDashboardCommandHandler(
    IDefaultRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<RemoveDashboardCommand, Result>
{
    public async Task<Result> Handle(RemoveDashboardCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.DataSourceId);

        await dataSourceRepository.DeleteAsync(id, token);
        return Result.Success();
    }
}