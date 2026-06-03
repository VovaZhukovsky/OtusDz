namespace ZeroAllocCore;

public class SimpleStore : ISimpleStore, IDisposable
{
    private long _setCount;
    private long _getCount;
    private long _deleteCount;
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
    private readonly Dictionary<string, byte[]> _store = new();
    private bool _isDisposed;
    public void Set(string? key, byte[]? value)
    {
        if (key is null && value is null)
            return;
        
        _lock.EnterWriteLock();
        try
        {
            _store[key] = value;
            Interlocked.Increment(ref _setCount);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
        
    }

    public byte[]? Get(string? key)
    {
        if (key is null)
            return null;
        
        _lock.EnterReadLock();
        try
        {
            var value = _store.GetValueOrDefault(key);
            Interlocked.Increment(ref _getCount);
            return value;
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public bool Remove(string? key)
    {
        if (key is null)
            return false;
        _lock.EnterWriteLock();

        try
        {
            var result =  _store.Remove(key);
            Interlocked.Increment(ref _deleteCount);
            return result;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public (long setCount, long getCount, long deleteCount) GetStatistics()
    {
        return (_setCount, _getCount, _deleteCount);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isManual)
    {
        if (_isDisposed)
            return;
        
        if (isManual)
        {
            _lock.Dispose();
        }

        _isDisposed = true;
    }
}