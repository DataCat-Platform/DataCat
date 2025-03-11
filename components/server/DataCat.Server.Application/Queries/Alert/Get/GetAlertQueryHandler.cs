namespace DataCat.Server.Application.Queries.Alert.Get;

public class GetAlertQueryHandler(
    IDefaultRepository<AlertEntity, Guid> alertRepository)
    : IRequestHandler<GetAlertQuery, Result<AlertEntity>>
{
    public async Task<Result<AlertEntity>> Handle(GetAlertQuery request, CancellationToken token)
    {
        var entity = await alertRepository.GetByIdAsync(request.AlertId, token);
        return entity is null 
            ? Result.Fail<AlertEntity>(AlertError.NotFound(request.AlertId)) 
            : Result<AlertEntity>.Success(entity);
    }
}