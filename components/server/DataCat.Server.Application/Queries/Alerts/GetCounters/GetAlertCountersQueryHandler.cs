namespace DataCat.Server.Application.Queries.Alerts.GetCounters;

public sealed class GetAlertCountersQueryHandler(
    IAlertRepository alertRepository)
    : IQueryHandler<GetAlertCountersQuery, List<AlertCounterResponse>>
{
    public async Task<Result<List<AlertCounterResponse>>> Handle(GetAlertCountersQuery request, CancellationToken cancellationToken)
    {
        var result = await alertRepository.GetAlertCountersAsync(cancellationToken);
        return Result.Success(result);
    }
}
