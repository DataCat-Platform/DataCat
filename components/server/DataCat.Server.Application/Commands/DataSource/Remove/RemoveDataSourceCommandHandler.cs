namespace DataCat.Server.Application.Commands.DataSource.Remove;

public sealed class RemoveDataSourceCommandHandler(
    IDataSourceRepository dataSourceRepository)
    : IRequestHandler<RemoveDataSourceCommand, Result>
{
    public async Task<Result> Handle(RemoveDataSourceCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.DataSourceId);

        await dataSourceRepository.DeleteAsync(id, token);
        return Result.Success();
    }
}