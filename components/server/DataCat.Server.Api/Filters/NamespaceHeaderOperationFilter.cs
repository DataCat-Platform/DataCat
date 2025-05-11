namespace DataCat.Server.Api.Filters;

public sealed class NamespaceHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = NamespaceEnricherMiddleware.HeaderName,
            In = ParameterLocation.Header,
            Description = "Optional namespace ID. If omitted, the default namespace is used.",
            Required = false
        });
    }
}