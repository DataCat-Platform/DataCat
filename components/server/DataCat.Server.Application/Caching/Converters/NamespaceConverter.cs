namespace DataCat.Server.Application.Caching.Converters;

public sealed class NamespaceConverter : JsonConverter<Namespace>
{
    private const string Id = "id";
    private const string Name = "name";
    private const string DashboardIds = "dashboardIds";
    
    public override Namespace? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        
        var id = root.GetProperty(Id).GetGuid();
        var name = root.GetProperty(Name).GetString() ?? string.Empty;;
        
        var dashboardIds = new List<Guid>();
        if (root.TryGetProperty(DashboardIds, out var dashboardIdsElement) && 
            dashboardIdsElement.ValueKind != JsonValueKind.Null)
        {
            foreach (var idElement in dashboardIdsElement.EnumerateArray())
            {
                if (idElement.TryGetGuid(out var dashboardId))
                {
                    dashboardIds.Add(dashboardId);
                }
            }
        }
        
        var result = Namespace.Create(id, name, dashboardIds);
    
        if (result.IsFailure)
        {
            throw new JsonException($"Failed to create Namespace: {string.Join("\n", result.Errors!.Select(x => x.ErrorMessage))}");
        }
        
        return result.Value;
    }

    public override void Write(Utf8JsonWriter writer, Namespace value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(Id, value.Id);
        writer.WriteString(Name, value.Name);
    
        writer.WritePropertyName(DashboardIds);
        writer.WriteStartArray();
        foreach (var dashboardId in value.DashboardIds)
        {
            writer.WriteStringValue(dashboardId);
        }
        writer.WriteEndArray();
    
        writer.WriteEndObject();
    }
}