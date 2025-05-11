namespace DataCat.Server.Application.Queries.Variables.GetForNamespace;

public sealed class GetVariablesForNamespaceQueryHandler(
    IVariableRepository variableRepository) : IQueryHandler<GetVariablesForNamespaceQuery, List<VariableResponse>>
{
    public async Task<Result<List<VariableResponse>>> Handle(GetVariablesForNamespaceQuery request, CancellationToken cancellationToken)
    {
        var variables = await variableRepository.GetAllAsyncForNamespaceAsync(request.NamespaceId, cancellationToken);
        return Result.Success(variables.Select(x => x.ToResponse()).ToList());
    }
}