namespace DataCat.Metrics.Prometheus.Core;

public sealed class PrometheusClient : IMetricsClient, IDisposable
{
    public string Name => PrometheusConstants.Prometheus;
    
    private readonly HttpClient _client;
    private readonly PrometheusSettings _settings;
    
    public PrometheusClient(HttpClient client, PrometheusSettings settings)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }
    
    public async Task<IEnumerable<MetricPoint>> QueryAsync(string query, CancellationToken token = default)
    {
        var url = $"{_settings.ServerUrl}/api/v1/query";
        var parameters = new Dictionary<string, string>
        {
            ["query"] = query,
            ["time"] = DateTime.UtcNow.ToUniversalTime().ToString(CultureInfo.InvariantCulture)
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
            ["start"] = start.ToUniversalTime().ToString(CultureInfo.InvariantCulture),
            ["end"] = end.ToUniversalTime().ToString(CultureInfo.InvariantCulture),
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
        
        if (!string.IsNullOrEmpty(_settings.AuthToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AuthToken);
        }

        var response = await _client.SendAsync(request, token);
        await HandleResponseErrors(response);

        var content = await response.Content.ReadAsStringAsync(token);
        return JsonSerializer.Deserialize<T>(content)!;
    }

    private async Task HandleResponseErrors(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        var content = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Prometheus API error: {response.StatusCode} - {content}");
    }

    private IEnumerable<MetricPoint> ProcessInstantQuery(PrometheusResponse response)
    {
        if (response.Data?.ResultType != "vector") yield break;

        foreach (var item in response.Data.Result)
        {
            yield return new MetricPoint
            {
                MetricName = item.Metric["__name__"],
                Value = double.Parse(item.Value[1].ToString()!),
                Timestamp = DateTimeOffset.FromUnixTimeSeconds((long)item.Value[0]).UtcDateTime,
                Labels = item.Metric
            };
        }
    }

    private IEnumerable<TimeSeries> ProcessRangeQuery(PrometheusResponse response)
    {
        if (response.Data?.ResultType != "matrix") yield break;

        foreach (var item in response.Data.Result)
        {
            var series = new TimeSeries
            {
                MetricName = item.Metric["__name__"],
                Labels = item.Metric
            };

            foreach (var value in item.Values)
            {
                series.Points.Add(new MetricPoint
                {
                    MetricName = item.Metric["__name__"],
                    Value = double.Parse(value[1].ToString()!),
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds((long)value[0]).UtcDateTime,
                    Labels = item.Metric
                });
            }

            yield return series;
        }
    }

    private IEnumerable<MetricSeries> ProcessSeriesResponse(PrometheusSeriesResponse response)
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

    public void Dispose()
    {
        _client?.Dispose();
    }
}