namespace DataCat.Server.Application.Caching.Converters;

public static class JsonConverterExtensions
{
    public static JsonSerializerOptions AddAllJsonConverters(this JsonSerializerOptions options, System.Reflection.Assembly assembly)
    {
        var converterTypes = assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false, BaseType.IsGenericType: true } &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(JsonConverter<>));

        foreach (var type in converterTypes)
        {
            var converter = (JsonConverter)Activator.CreateInstance(type)!;
            options.Converters.Add(converter);
        }

        return options;
    }
}