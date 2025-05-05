namespace DataCat.Server.Application.Queries.Variables.GetById;

public sealed class GetVariableByIdQueryHandler(
    IRepository<Variable, Guid> repository) : IQueryHandler<GetVariableByIdQuery, VariableResponse>
{
    public async Task<Result<VariableResponse>> Handle(GetVariableByIdQuery request, CancellationToken cancellationToken)
    {
        var variable = await repository.GetByIdAsync(request.Id, cancellationToken);
        return variable is null 
            ? Result.Fail<VariableResponse>("Variable not found") 
            : Result.Success(variable.ToResponse());
    }
}