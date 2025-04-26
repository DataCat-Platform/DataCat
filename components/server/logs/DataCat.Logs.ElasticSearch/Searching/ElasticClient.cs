namespace DataCat.Logs.ElasticSearch.Searching;

public sealed partial class ElasticClient : ILogsClient, IDisposable
{
    private readonly ElasticsearchClient _client;
    private readonly string _indexPattern;

    public string Name => ElasticSearchConstants.ElasticSearch;

    public ElasticClient(ElasticsearchClient client, string indexPattern)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _indexPattern = indexPattern ?? throw new ArgumentNullException(nameof(indexPattern));
    }
    
    public async Task<Page<LogEntry>> SearchAsync(LogSearchQuery query, CancellationToken token = default)
    {
        var searchRequest = new SearchRequest(_indexPattern)
        {
            From = (query.Page - 1) * query.PageSize,
            Size = query.PageSize,
            Query = BuildQuery(query),
            Sort = BuildSort(query)
        };

        var response = await _client.SearchAsync<ElasticSearchDocument>(searchRequest, token);

        HandleExceptionIfExists(response);

        return new Page<LogEntry>(
            Items: response.Documents.Select(x => x.FromElasticDocument()),
            TotalCount: response.Total,
            PageNumber: query.Page,
            PageSize: query.PageSize
        );
    }

    public async Task<IReadOnlyCollection<string>> GetDistinctValuesAsync(string fieldName, LogSearchQuery? baseQuery = null, CancellationToken token = default)
    {
        var searchRequest = new SearchRequest(_indexPattern)
        {
            Size = 0, // don't want to receive the documents themselves, only the aggregation
            Query = baseQuery is not null ? BuildQuery(baseQuery) : new MatchAllQuery(),
            Aggregations = new Dictionary<string, Aggregation>
            {
                ["distinct_values"] = new TermsAggregation
                {
                    Field = fieldName,
                    Size = 1000
                }
            }
        };

        var response = await _client.SearchAsync<ElasticSearchDocument>(searchRequest, token);

        HandleExceptionIfExists(response);

        if (response.Aggregations != null &&
            response.Aggregations.TryGetValue("distinct_values", out var agg) &&
            agg is StringTermsAggregate { Buckets: not null } stringTerms)
        {
            return stringTerms.Buckets
                .Select(b => b.Key.ToString())
                .ToArray();
        }

        return Array.Empty<string>();
    }

    public async Task<TimeSeriesSummary> GetTimeSeriesSummaryAsync(
        LogSearchQuery query,
        TimeSpan interval,
        CancellationToken token = default)
    {
        var searchRequest = new SearchRequest(_indexPattern)
        {
            Size = 0, // Только агрегация
            Query = BuildQuery(query),
            Aggregations = new Dictionary<string, Aggregation>
            {
                ["logs_over_time"] = new DateHistogramAggregation
                {
                    Field = LogFields.Timestamp!,
                    FixedInterval = interval,
                    Format = "yyyy-MM-dd'T'HH:mm:ssZ"
                },
            }
        };

        var response = await _client.SearchAsync<ElasticSearchDocument>(searchRequest, token);

        HandleExceptionIfExists(response);

        var histogram = response.Aggregations?
            .GetDateHistogram("logs_over_time");

        if (histogram?.Buckets is null)
        {
            return new TimeSeriesSummary([]);
        }

        var points = histogram.Buckets.Select(b =>
        {
            var intervalStart = DateTime.Parse(b.KeyAsString!, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
            var intervalEnd = intervalStart.Add(interval);
            var count = b.DocCount;

            return new TimeSeriesPoint(
                IntervalStart: intervalStart,
                IntervalEnd: intervalEnd,
                Count: count
            );
        }).ToList();

        return new TimeSeriesSummary(points);
    }

    private static Query BuildQuery(LogSearchQuery query)
    {
        var queries = new List<Query>();

        if (!string.IsNullOrEmpty(query.TraceId))
        {
            queries.Add(new TermQuery(LogFields.TraceId!) { Value = query.TraceId });
        }

        if (!string.IsNullOrEmpty(query.Severity))
        {
            queries.Add(new TermQuery(LogFields.Severity!) { Value = query.Severity });
        }

        if (!string.IsNullOrEmpty(query.ServiceName))
        {
            queries.Add(new TermQuery(LogFields.ServiceName!) { Value = query.ServiceName });
        }

        if (query.From.HasValue || query.To.HasValue)
        {
            queries.Add(new DateRangeQuery(LogFields.Timestamp!)
            {
                Gte = query.From!.Value,
                Lte = query.To!.Value,
            });
        }

        if (query.CustomFilters is not null)
        {
            var filters = query.CustomFilters
                .Where(filter => !string.IsNullOrEmpty(filter.Value))
                .Select(filter => 
                {
                    // if use `.keyword` prefix
                    if (filter.Key.EndsWith(".keyword", StringComparison.OrdinalIgnoreCase))
                    {
                        return (Query)new TermQuery(filter.Key!) 
                        { 
                            Value = filter.Value 
                        };
                    }
                    // otherwise use match query filter
                    else 
                    {
                        return new MatchQuery(filter.Key!)
                        {
                            Query = filter.Value,
                            Analyzer = "standard" 
                        };
                    }
                })
                .ToArray();
            
            queries.AddRange(filters.Select(x => x));
        }

        return queries.Count switch
        {
            0 => new MatchAllQuery(),
            1 => queries[0],
            _ => new BoolQuery { Must = queries }
        };
    }
    
    private static SortOptions[] BuildSort(LogSearchQuery query)
    {
        if (string.IsNullOrEmpty(query.SortField))
        {
            return [];
        }

        var sortField = new FieldSort
        {
            Order = query.SortAscending ? SortOrder.Asc : SortOrder.Desc
        };
        
        var sortOptions = SortOptions.Field(
            new Field(query.SortField), 
            sortField
        );

        return [sortOptions];
    }

    private static void HandleExceptionIfExists<T>(SearchResponse<T> response)
    {
        if (response.IsValidResponse)
        {
            return;
        }

        if (response.DebugInformation.Contains("No mapping found for"))
        {
            const string pattern = @"No mapping found for \[(.+?)\] in order to sort on";
            var match = SortMappingErrorRegex().Match(response.DebugInformation);

            if (!match.Success)
            {
                throw new InvalidOperationException(response.DebugInformation);
            }
            
            var errorMessage = match.Groups[0].Value;
            throw new InvalidOperationException(errorMessage);
        }
        
        throw new InvalidOperationException(response.DebugInformation);    
    }

    public void Dispose()
    {
    }

    [GeneratedRegex(@"No mapping found for \[(.+?)\] in order to sort on")]
    private static partial Regex SortMappingErrorRegex();

}