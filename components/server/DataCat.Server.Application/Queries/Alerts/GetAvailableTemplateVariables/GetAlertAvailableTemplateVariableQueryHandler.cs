namespace DataCat.Server.Application.Queries.Alerts.GetAvailableTemplateVariables;

public sealed class GetAlertAvailableTemplateVariableQueryHandler
    : IQueryHandler<GetAlertAvailableTemplateVariableQuery, List<string>>
{
    public Task<Result<List<string>>> Handle(GetAlertAvailableTemplateVariableQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success(AlertTemplateRenderer.GetAvailablePlaceholders()));
    }
}