namespace DataCat.Server.Application.Queries.Alerts.Get;

public class GetAlertQueryHandler(
    IRepository<Alert, Guid> alertRepository)
    : IRequestHandler<GetAlertQuery, Result<GetAlertResponse>>
{
    public async Task<Result<GetAlertResponse>> Handle(GetAlertQuery request, CancellationToken token)
    {
        var entity = await alertRepository.GetByIdAsync(request.AlertId, token);
        
        return entity is null 
            ? Result.Fail<GetAlertResponse>(AlertError.NotFound(request.AlertId)) 
            : Result<GetAlertResponse>.Success(entity.ToResponse());
    }
}