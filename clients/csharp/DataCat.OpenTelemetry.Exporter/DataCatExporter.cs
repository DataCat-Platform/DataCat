namespace DataCat.OpenTelemetry.Exporter;

public class DataCatExporter : BaseExporter<Metric>
{
    private readonly GrpcChannel _channel;
    private bool _disposed;
    private readonly DataCatMetricExporter.DataCatMetricExporterClient _dataCatClient;

    public DataCatExporter(DataCatExporterOptions options)
    {
        NullGuard.ThrowIfNull(options);
        
        options.ChannelOptions ??= new GrpcChannelOptions();
        _channel = GrpcChannel.ForAddress(options.Address, options.ChannelOptions);
        _dataCatClient = new DataCatMetricExporter.DataCatMetricExporterClient(_channel);
    }

    public override ExportResult Export(in Batch<Metric> batch)
    {
        using var scope = SuppressInstrumentationScope.Begin();

        try
        {
            if (batch.Count == 0)
            {
                return ExportResult.Success;
            }
            
            // TODO: add logic with custom OTEL resource
            
            // send metrics via grpc
            ExportMetrics(batch);
        }
        catch (Exception ex)
        {
            DataCatExporterEventSource.Log.FailedExport(ex);
            return ExportResult.Failure;
        }
        
        return ExportResult.Success;
    }

    private void ExportMetrics(in Batch<Metric> batch)
    {
        var dataCatMetricsRequest = new SendMetricsRequest();
        foreach (var metric in batch)
        {
            var convertedMetric = ConvertMetricToDataCatConventions(metric);
            if (convertedMetric.DataPointsCase != DataCat.Export.Metrics.V1.Metric.DataPointsOneofCase.None)
            {
                dataCatMetricsRequest.Metrics.Add(convertedMetric);            
            }
        }

        _dataCatClient.SendMetricsAsync(dataCatMetricsRequest).GetAwaiter().GetResult();
    }

    private static Export.Metrics.V1.Metric ConvertMetricToDataCatConventions(Metric metric)
    {
        var dataCatMetrics = new Export.Metrics.V1.Metric();
        
        if (metric.MeterTags is not null)
        {
            var list = metric.MeterTags?
                .Where(tag => tag.Value is not null)
                .Select(tag => new Tag { Key = tag.Key, Value = tag.Value!.ToString() })
                .ToList();
            dataCatMetrics.Tags.AddRange(list);
        }
        
        switch (metric.MetricType)
        {
            case MetricType.LongSum:
                dataCatMetrics.Counter ??= new CounterDataPoints();
                ProcessMetricPoints(metric, 
                    metricPoint => new CounterDataPoint
                    {
                        Timestamp = metricPoint.EndTime.ToTimestamp(),
                        Value = metricPoint.GetSumLong()
                    }, 
                    dataCatMetrics.Counter.DataPoints);
                break;

            case MetricType.DoubleSum:
                dataCatMetrics.Counter ??= new CounterDataPoints();
                ProcessMetricPoints(metric, 
                    metricPoint => new CounterDataPoint
                    {
                        Timestamp = metricPoint.EndTime.ToTimestamp(),
                        Value = (long)metricPoint.GetSumDouble()
                    }, 
                    dataCatMetrics.Counter.DataPoints);
                break;

            case MetricType.LongGauge:
                dataCatMetrics.Gauge ??= new GaugeDataPoints();
                ProcessMetricPoints(metric, 
                    metricPoint => new GaugeDataPoint
                    {
                        Timestamp = metricPoint.EndTime.ToTimestamp(),
                        Value = metricPoint.GetGaugeLastValueLong()
                    }, 
                    dataCatMetrics.Gauge.DataPoints);
                break;

            case MetricType.DoubleGauge:
                dataCatMetrics.Gauge ??= new GaugeDataPoints();
                ProcessMetricPoints(metric, 
                    metricPoint => new GaugeDataPoint
                    {
                        Timestamp = metricPoint.EndTime.ToTimestamp(),
                        Value = metricPoint.GetGaugeLastValueDouble()
                    }, 
                    dataCatMetrics.Gauge.DataPoints);
                break;

            case MetricType.Histogram:
            case MetricType.ExponentialHistogram:
                dataCatMetrics.Histogram ??= new HistogramDataPoints();
                ProcessMetricPoints(metric, metricPoint =>
                    {
                        var histogramDataPoint = new HistogramDataPoint
                        {
                            StartTimestamp = metricPoint.StartTime.ToTimestamp(),
                            EndTimestamp = metricPoint.EndTime.ToTimestamp(),
                            Sum = metricPoint.GetHistogramSum(),
                            Count = metricPoint.GetHistogramCount()
                        };

                        // Попытка получить минимальное и максимальное значения
                        if (!metricPoint.TryGetHistogramMinMaxValues(out var min, out var max))
                        {
                            return histogramDataPoint;
                        }
                        histogramDataPoint.Min = min;
                        histogramDataPoint.Max = max;

                        return histogramDataPoint;
                    }, 
                    dataCatMetrics.Histogram.DataPoints);
                break;

            case MetricType.LongSumNonMonotonic:
            case MetricType.DoubleSumNonMonotonic:
            default:
                Console.WriteLine($"[UNSUPPORTED TYPE] metric type: {metric.MetricType}");
                break;
        }

        return dataCatMetrics;
    }
    
    /// <summary>
    /// Processes metric points and adds them to the data points collection.
    /// </summary>
    /// <typeparam name="TDataPoint">The type of the data point.</typeparam>
    /// <param name="metric">The metric object.</param>
    /// <param name="createDataPoint">A function to create a data point from a metric point.</param>
    /// <param name="dataPoints">The collection to which the data points will be added.</param>
    private static void ProcessMetricPoints<TDataPoint>(
        Metric metric,
        Func<MetricPoint, TDataPoint> createDataPoint,
        ICollection<TDataPoint> dataPoints)
    {
        foreach (ref readonly var metricPoint in metric.GetMetricPoints())
        {
            var dataPoint = createDataPoint(metricPoint);
            dataPoints.Add(dataPoint);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                DataCatExporterEventSource.Log.DisposedObject(nameof(DataCatExporter));
                _channel?.Dispose();
            }
            _disposed = true;
        }

        base.Dispose(disposing);
    }
}