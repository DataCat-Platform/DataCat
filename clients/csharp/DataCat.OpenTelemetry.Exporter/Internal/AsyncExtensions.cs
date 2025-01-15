namespace DataCat.OpenTelemetry.Exporter.Internal;

public static class AsyncExtensions
{
    public static T EnsureCompleted<T>(this Task<T> task)
    {
        return task.GetAwaiter().GetResult();
    }

    public static void EnsureCompleted(this Task task)
    {
        task.GetAwaiter().GetResult();
    }
}