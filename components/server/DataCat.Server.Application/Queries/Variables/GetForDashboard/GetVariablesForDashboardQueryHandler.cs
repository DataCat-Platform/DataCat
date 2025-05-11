namespace DataCat.Server.Application.Queries.Variables.GetForDashboard;

public sealed class GetVariablesForDashboardQueryHandler(
    IVariableRepository variableRepository) : IQueryHandler<GetVariablesForDashboardQuery, List<VariableResponse>>
{
    public async Task<Result<List<VariableResponse>>> Handle(GetVariablesForDashboardQuery request, CancellationToken cancellationToken)
    {
        var variables = await variableRepository.GetAllAsyncForDashboardAsync(request.DashboardId, cancellationToken);
        return Result.Success(variables.Select(x => x.ToResponse()).ToList());
    }
}