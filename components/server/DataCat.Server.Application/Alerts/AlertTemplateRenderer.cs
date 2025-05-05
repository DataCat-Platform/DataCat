namespace DataCat.Server.Application.Alerts;

public static partial class AlertTemplateRenderer
{
    private static readonly IReadOnlyDictionary<string, Func<Alert, string>> Placeholders = new Dictionary<string, Func<Alert, string>>
    {
        [".id"] = alert => alert.Id.ToString(),
        [".status"] = alert => alert.Status.Name,
        [".condition_query"] = alert => alert.ConditionQuery.RawQuery,
        [".previous_execution"] = alert => alert.PreviousExecution.DateTime.ToString("dd/MM/yyyy HH:mm"),
        [".next_execution"] = alert => alert.NextExecution.DateTime.ToString("dd/MM/yyyy HH:mm"),
        [".repeat_interval"] = alert => alert.Schedule.RepeatInterval.ToString(),
        [".wait_time_before_alerting"] = alert => alert.Schedule.WaitTimeBeforeAlerting.ToString(),
    };
    
    public static string Render(string template, Alert alert)
    {
        return PlaceholderRegex().Replace(template, match =>
        {
            var key = match.Groups[1].Value.Trim();
            return Placeholders.TryGetValue(key, out var valueFactory)
                ? valueFactory(alert)
                : match.Value;
        });
    }
    
    public static List<string> GetAvailablePlaceholders() => Placeholders.Keys.ToList();

    [GeneratedRegex(@"\{\s*(\.\w+)\s*\}", RegexOptions.Compiled)]
    private static partial Regex PlaceholderRegex();
}