namespace DataCat.Server.Application.Services;

public interface IVariableService
{
    Task<string> ResolveQueryVariablesAsync(string rawQuery, Guid namespaceId, Guid? dashboardId = null, CancellationToken token = default);
}

public sealed partial class VariableService(
    IVariableRepository variableRepository,
    ICacheService cacheService) 
    : IVariableService
{
    public async Task<string> ResolveQueryVariablesAsync(
        string rawQuery,
        Guid namespaceId,
        Guid? dashboardId = null,
        CancellationToken token = default)
    {
        var namespaceVariables = await variableRepository.GetAllAsyncForNamespaceAsync(namespaceId, token);

        var dashboardVariables = dashboardId.HasValue 
            ? await variableRepository.GetAllAsyncForDashboardAsync(dashboardId.Value, token)
            : [];

        var variablesDict = namespaceVariables
            .Concat(dashboardVariables)
            .ToLookup(v => v.Placeholder)
            .ToDictionary(
                g => g.Key,
                g => g.Last().Value); // dashboard vars have more priority than namespace's vars

        return PlaceholderRegex().Replace(rawQuery, match =>
        {
            var placeholder = match.Groups[1].Value;
            return variablesDict.TryGetValue(placeholder, out var value) 
                ? value 
                : match.Value;
        });
    }

    [GeneratedRegex(@"\{\s*\.(\w+)\s*\}", RegexOptions.Compiled)]
    private static partial Regex PlaceholderRegex();
}