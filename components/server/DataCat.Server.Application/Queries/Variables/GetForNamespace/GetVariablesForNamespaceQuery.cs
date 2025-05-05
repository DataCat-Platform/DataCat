namespace DataCat.Server.Application.Queries.Variables.GetForNamespace;

public sealed record GetVariablesForNamespaceQuery(Guid NamespaceId) : IQuery<List<VariableResponse>>, IAuthorizedQuery;