namespace DataCat.Server.Application.Queries.Variables.GetById;

public sealed record GetVariableByIdQuery(Guid Id) : IQuery<VariableResponse>, IAuthorizedQuery;
