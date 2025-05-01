namespace DataCat.Metrics.Prometheus.Core;

public sealed class PrometheusClient : IMetricsClient
{
    public string Name => PrometheusConstants.Prometheus;
    
    private readonly HttpClient _client;
    private readonly PrometheusSettings _settings;
    
    public PrometheusClient(HttpClient client, PrometheusSettings settings)
    {
        settings.ThrowIfIsInvalid();
        
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<IEnumerable<MetricPoint>> QueryAsync(string query, CancellationToken token = default)
    {
        var url = $"{_settings.ServerUrl}/api/v1/query";
        var parameters = new Dictionary<string, string>
        {
            ["query"] = query,
            ["time"] = DateTime.UtcNow.ToString("o")
        };

        var response = await ExecuteRequest<PrometheusResponse>(url, parameters, token);
        return ProcessInstantQuery(response);
    }

    public async Task<IEnumerable<TimeSeries>> RangeQueryAsync(
        string query,
        DateTime start,
        DateTime end,
        TimeSpan step,
        CancellationToken token = default)
    {
        var url = $"{_settings.ServerUrl}/api/v1/query_range";
        var parameters = new Dictionary<string, string>
        {
            ["query"] = query,
            ["start"] = start.ToUniversalTime().ToString("o"),
            ["end"] = end.ToUniversalTime().ToString("o"),
            ["step"] = step.TotalSeconds.ToString("0")
        };

        var response = await ExecuteRequest<PrometheusResponse>(url, parameters, token);
        return ProcessRangeQuery(response);
    }

    public async Task<IEnumerable<MetricSeries>> ListSeriesAsync(
        string matchExpression,
        CancellationToken token = default)
    {
        var url = $"{_settings.ServerUrl}/api/v1/series";
        var parameters = new Dictionary<string, string>
        {
            ["match[]"] = matchExpression
        };

        var response = await ExecuteRequest<PrometheusSeriesResponse>(url, parameters, token);
        return ProcessSeriesResponse(response);
    }

    public async Task<MetricPoint?> GetLatestMetricAsync(
        string metricName,
        CancellationToken token = default)
    {
        var query = $"{metricName}";
        var result = await QueryAsync(query, token);
        return result.OrderByDescending(p => p.Timestamp).FirstOrDefault();
    }

    public async Task<bool> CheckAlertTriggerAsync(
        string rawQuery,
        CancellationToken token = default)
    {
        try
        {
            var result = await QueryAsync(rawQuery, token);
            return result.Any(p => p.Value > 0);
        }
        catch
        {
            return false;
        }
    }

    private async Task<T> ExecuteRequest<T>(
        string url,
        Dictionary<string, string> parameters,
        CancellationToken token)
    {
        var requestUri = QueryHelpers.AddQueryString(url, parameters!);
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        
        ApplyAuthentication(request);

        var response = await _client.SendAsync(request, token);
        await HandleResponseErrors(response);

        var content = await response.Content.ReadAsStringAsync(token);
        return JsonSerializer.Deserialize<T>(content)!;
    }

    private static async Task HandleResponseErrors(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Prometheus API error: {response.StatusCode} - {content}");
    }

    private static IEnumerable<MetricPoint> ProcessInstantQuery(PrometheusResponse response, string defaultMetricName = "unnamed")
    {
        if (response.Data?.ResultType != "vector" || response.Data?.Result == null)
        {
            yield break;
        }

        foreach (var item in response.Data.Result)
        {
            if (item?.Value is null || item.Value.Length < 2)
                continue;

            // parse value of metric
            if (!double.TryParse(item.Value[1]?.ToString(), out var metricValue))
                continue;

            // parse timestamp of metric
            if (!double.TryParse(item.Value[0]?.ToString(), out var unixTimestamp))
                continue;
            
            var timestamp = DateTimeOffset.FromUnixTimeMilliseconds((long)(unixTimestamp * 1000)).UtcDateTime;
            
            yield return new MetricPoint
            {
                MetricName = defaultMetricName,
                Value = metricValue,
                Timestamp = timestamp,
                Labels = item.Metric
            };
        }
    }

    private static IEnumerable<TimeSeries> ProcessRangeQuery(PrometheusResponse response, string defaultMetricName = "unnamed")
    {
        if (response.Data?.ResultType != "matrix" || response.Data?.Result == null)
        {
            yield break;
        }

        foreach (var item in response.Data.Result)
        {
            if (item?.Values == null)
                continue;

            string metricName = item.Metric?.TryGetValue("__name__", out var name) == true 
                ? name 
                : defaultMetricName;

            var series = new TimeSeries
            {
                MetricName = metricName,
                Labels = item.Metric ?? new Dictionary<string, string>()
            };

            foreach (var value in item.Values)
            {
                if (value is null || value.Length < 2)
                    continue;

                // parse value of metric
                if (!double.TryParse(value[1]?.ToString(), out var metricValue))
                    continue;

                // parse timestamp of metric
                if (!double.TryParse(value[0]?.ToString(), out var unixTimestamp))
                    continue;

                series.Points.Add(new MetricPoint
                {
                    MetricName = metricName,
                    Value = metricValue,
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds((long)(unixTimestamp * 1000)).UtcDateTime,
                    Labels = item.Metric ?? new Dictionary<string, string>()
                });
            }

            if (series.Points.Count > 0)
            {
                yield return series;
            }
        }
    }

    private static IEnumerable<MetricSeries> ProcessSeriesResponse(PrometheusSeriesResponse response)
    {
        foreach (var item in response.Data)
        {
            yield return new MetricSeries
            {
                MetricName = item["__name__"],
                Labels = item
            };
        }
    }
    
    private void ApplyAuthentication(HttpRequestMessage request)
    {
        switch (_settings.AuthType)
        {
            case PrometheusAuthType.Basic:
                var authValue = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes($"{_settings.Username}:{_settings.Password}"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);
                break;
            
            case PrometheusAuthType.Bearer:
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AuthToken);
                break;
        }
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}