namespace DataCat.Traces.Jaeger.Core;

public class JaegerClient : ITracesClient
{
    public string Name => JaegerConstants.Jaeger;
    
    private readonly HttpClient _client;
    private readonly JaegerSettings _settings;
    private readonly JsonSerializerOptions _jsonOptions;

    public JaegerClient(HttpClient client, JaegerSettings settings)
    {
        settings.ThrowIfIsInvalid();
        
        _client = client;
        _settings = settings;
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };
    }
    
    public async Task<IEnumerable<string>> GetServicesAsync(CancellationToken token = default)
    {
        var response = await ExecuteRequest<JaegerListResponse<string>>("/api/services", null, token);
        return response?.Data ?? Enumerable.Empty<string>();
    }

    public async Task<IEnumerable<string>> GetOperationsAsync(string serviceName, CancellationToken token = default)
    {
        var response = await ExecuteRequest<JaegerListResponse<string>>(
            $"/api/services/{Uri.EscapeDataString(serviceName)}/operations",
            null,
            token);

        return response?.Data ?? Enumerable.Empty<string>();
    }

    public async Task<IEnumerable<TraceEntry>> FindTracesAsync(
        string service, 
        DateTime start, 
        DateTime end, 
        string? operation = null,
        int? limit = null,
        TimeSpan? minDuration = null,
        TimeSpan? maxDuration = null,
        Dictionary<string, object>? tags = null,
        CancellationToken token = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["service"] = service,
            ["start"] = ToMicroseconds(start).ToString(),
            ["end"] = ToMicroseconds(end).ToString()
        };

        if (!string.IsNullOrEmpty(operation))
        {
            parameters["operation"] = operation;
        }

        if (limit.HasValue)
        {
            parameters["limit"] = limit.Value.ToString();
        }

        if (minDuration.HasValue)
        {
            parameters["minDuration"] = ((long)minDuration.Value.TotalMilliseconds * 1000).ToString();
        }

        if (maxDuration.HasValue)
        {
            parameters["maxDuration"] = ((long)maxDuration.Value.TotalMilliseconds * 1000).ToString();
        }

        if (tags?.Count > 0)
        {
            parameters["tags"] = Uri.EscapeDataString(JsonSerializer.Serialize(tags));
        }

        var response = await ExecuteRequest<JaegerTracesResponse>("/api/traces", parameters, token);
        return MapTracesResponse(response);
    }

    public async Task<TraceEntry?> GetTraceAsync(string traceId, CancellationToken token = default)
    {
        var response = await ExecuteRequest<JaegerTracesResponse>(
            $"/api/traces/{Uri.EscapeDataString(traceId)}",
            null,
            token);

        return MapTracesResponse(response).FirstOrDefault();
    }

    private async Task<T> ExecuteRequest<T>(
        string endpoint,
        Dictionary<string, string>? parameters,
        CancellationToken token)
    {
        var url = $"{_settings.ServerUrl}{endpoint}";
        if (parameters?.Count > 0)
            url = QueryHelpers.AddQueryString(url, parameters!);

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        ApplyAuthentication(request);

        var response = await _client.SendAsync(request, token);
        await HandleResponseErrors(response);

        var content = await response.Content.ReadAsStringAsync(token);
        return JsonSerializer.Deserialize<T>(content, _jsonOptions)!;
    }

    private void ApplyAuthentication(HttpRequestMessage request)
    {
        switch (_settings.AuthType)
        {
            case JaegerAuthType.Basic:
                var authValue = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);
                break;
            
            case JaegerAuthType.Bearer:
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AuthToken);
                break;
        }
    }

    private static async Task HandleResponseErrors(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Jaeger API error: {response.StatusCode} - {content}");
        }
    }

    private static List<TraceEntry> MapTracesResponse(JaegerTracesResponse? response)
    {
        if (response?.Data is null)
        {
            return [];
        }

        return response.Data.Select(t => new TraceEntry
        {
            TraceId = t.TraceId,
            Spans = t.Spans.Select(MapSpan).ToList(),
            Processes = t.Processes.ToDictionary(
                p => p.Key,
                p => new ProcessEntry
                {
                    ServiceName = p.Value.ServiceName,
                    Tags = MapTags(p.Value.Tags)
                })
        }).ToList();
    }

    private static SpanEntry MapSpan(JaegerSpan span)
    {
        return new SpanEntry
        {
            TraceId = span.TraceId,
            SpanId = span.SpanId,
            OperationName = span.OperationName,
            StartTime = FromMicroseconds(span.StartTime),
            Duration = TimeSpan.FromMilliseconds(span.Duration / 1000),
            Tags = MapTags(span.Tags),
            References = span.References.Select(MapReference).ToList(),
            ProcessId = span.ProcessId
        };
    }

    private static Dictionary<string, object> MapTags(IEnumerable<JaegerTag> tags)
    {
        return tags.ToDictionary(
            t => t.Key,
            t => (object)(t.Type switch
            {
                "string" => t.Value.GetString(),
                "int64" => t.Value.GetInt64(),
                "bool" => t.Value.GetBoolean(),
                "double" => t.Value.GetDouble(),
                _ => t.Value.ToString()
            })!);
    }

    private static SpanReference MapReference(JaegerSpanReference reference)
    {
        return new SpanReference
        {
            TraceId = reference.TraceId,
            SpanId = reference.SpanId,
            ReferenceType = reference.RefType switch
            {
                "CHILD_OF" => SpanReferenceType.ChildOf,
                "FOLLOWS_FROM" => SpanReferenceType.FollowsFrom,
                _ => SpanReferenceType.Other
            }
        };
    }

    private static long ToMicroseconds(DateTime dateTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)(dateTime.ToUniversalTime() - epoch).TotalMilliseconds * 1000;
    }

    private static DateTime FromMicroseconds(long microseconds)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddTicks(microseconds * 10);
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}