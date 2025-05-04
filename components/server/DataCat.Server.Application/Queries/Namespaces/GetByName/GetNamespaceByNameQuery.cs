namespace DataCat.Server.Application.Queries.Namespaces.GetByName;

public sealed record GetNamespaceByNameQuery(string Name) : IQuery<NamespaceByNameResponse>, IAuthorizedQuery;
