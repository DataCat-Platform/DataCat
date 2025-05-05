namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record VariableResponse(
    Guid Id,
    string Placeholder,
    string Value,
    Guid NamespaceId,
    Guid? DashboardId);

public static class VariableResponseExtensions
{
    public static VariableResponse ToResponse(this Variable variable)
    {
        return new VariableResponse(
            variable.Id, 
            variable.Placeholder,
            variable.Value,
            variable.NamespaceId,
            variable.DashboardId
        );
    }
}
