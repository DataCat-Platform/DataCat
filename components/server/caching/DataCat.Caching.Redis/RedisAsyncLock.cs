namespace DataCat.Caching.Redis;

public sealed class RedisAsyncLock : IDisposable
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();
    private readonly TimeSpan _lockTimeout = TimeSpan.FromSeconds(5);
    private bool _disposed;

    public async Task<IDisposable> AcquireAsync(string key)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(RedisAsyncLock));
        }

        var semaphore = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        if (!await semaphore.WaitAsync(_lockTimeout))
        {
            throw new TimeoutException($"Failed to acquire lock for key '{key}'");
        }

        return new LockReleaser(key, semaphore, RemoveLock);
    }

    private void RemoveLock(string key) => _locks.TryRemove(key, out _);

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        
        _disposed = true;
        foreach (var semaphore in _locks.Values)
        {
            try
            {
                semaphore.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during disposing {nameof(RedisAsyncLock)}, Exception: {ex.Message}");
            }
        }
        _locks.Clear();
    }

    private sealed class LockReleaser(string key, SemaphoreSlim semaphore, Action<string> releaseAction)
        : IDisposable
    {
        private bool _released;

        public void Dispose()
        {
            if (_released)
            {
                return;
            }
            
            semaphore.Release();
            releaseAction(key);
            _released = true;
        }
    }
}