namespace DataCat.Server.Application.Queries.Namespaces.GetByName;

public sealed record NamespaceByNameResponse(string Name, List<DashboardResponse> Dashboards);
